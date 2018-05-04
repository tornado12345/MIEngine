#! /bin/bash

# Location of the script
__ScriptDirectory=

# ClrDbg Meta Version. It could be something like 'latest', 'vs2015', or a fully specified version. 
__ClrDbgMetaVersion=

# Install directory of the clrdbg relative to the script. 
__InstallLocation=

# When SkipDownloads is set to true, no access to internet is made.
__SkipDownloads=false

# Launches ClrDbg after downloading/upgrading.
__LaunchClrDbg=false

# Removes existing installation of ClrDbg in the Install Location.
__RemoveExistingOnUpgrade=false

# Internal, fully specified version of the ClrDbg. Computed when the meta version is used.
__ClrDbgVersion=

# RuntimeID of dotnet
__RuntimeID=

# Gets the script directory
get_script_directory()
{
    pushd $(dirname "$0") > /dev/null 2>&1
    __ScriptDirectory=$(pwd)
    popd > /dev/null 2>&1
}

print_help()
{
    echo 'GetClrDbg.sh [-usdh] -v V [-l L] [-r R]'
    echo ''
    echo 'This script downloads and configures clrdbg, the Cross Platform .NET Debugger'
    echo '-u    Deletes the existing installation directory of the debugger before installing the current version.'
    echo '-s    Skips any steps which requires downloading from the internet.'
    echo '-d    Launches debugger after the script completion.'
    echo '-h    Prints usage information.'
    echo '-v V  Version V can be "latest" or a version number such as 15.0.25930.0'
    echo '-l L  Location L where the debugger should be installed. Can be absolute or relative'
    echo '-r R  Debugger for the RuntimeID will be installed'
    echo ''
    echo 'Legacy commandline'
    echo '  GetClrDbg.sh <version> [<install path>]'   
    echo '  If <install path> is not specified, clrdbg will be installed to the directory'
    echo '  from which this script was executed.' 
    echo '  <version> can be "latest" or a version number such as 15.0.25930.0'
}

# Set the __RuntimeID by reading the contents of /etc/os-release. 
# This logic is identical to the one used by C# extensions, make sure they are kept in sync.
get_dotnet_runtime_id()
{
    # Sample content of /etc/os-release looks like this
    # NAME="Ubuntu"
    # VERSION="14.04.5 LTS, Trusty Tahr"
    # ID=ubuntu
    # ID_LIKE=debian
    # PRETTY_NAME="Ubuntu 14.04.5 LTS"
    # VERSION_ID="14.04"
    # HOME_URL="http://www.ubuntu.com/"
    # SUPPORT_URL="http://help.ubuntu.com/"
    # BUG_REPORT_URL="http://bugs.launchpad.net/ubuntu/"

    PlatformID=$(cat /etc/os-release | awk '{split($0,a,"="); if (a[1] == "ID") print a[2]}' | tr -d \" | tr -d '[[:space:]]')
    PlatformVersionID=$(cat /etc/os-release | awk '{split($0,a,"="); if (a[1] == "VERSION_ID") print a[2]}' | tr -d \" | tr -d '[[:space:]]')

    case $PlatformID in
        ubuntu) 
            if [[ "$PlatformVersionID" == 14* ]]; then
                __RuntimeID=ubuntu.14.04-x64
            elif [[ "$PlatformVersionID" == 16.04 ]]; then
                __RuntimeID=ubuntu.16.04-x64
            elif [[ "$PlatformVersionID" == 16.10 ]]; then
                __RuntimeID=ubuntu.16.10-x64
            fi
            ;;
        centos)
            __RuntimeID=centos.7-x64
            ;;
        fedora)
            if [[ "$PlatformVersionID" == 23 ]]; then
                __RuntimeID=fedora.23-x64
            elif [[ "$PlatformVersionID" == 24 ]]; then
                __RuntimeID=fedora.24-x64
            fi
            ;;
        opensuse)
            if [[ "$PlatformVersionID" == 13.2 ]]; then
                __RuntimeID=opensuse.13.2-x64
            elif [[ "$PlatformVersionID" == 42.1 ]]; then
                __RuntimeID=opensuse.42.1-x64
            fi
            ;;
        rhel)
            __RuntimeID=rhel.7.2-x64
            ;;
        debian)
            __RuntimeID=debian.8-x64
            ;;
        ol)
            __RuntimeID=centos.7-x64
            ;;
        elementaryOS)
            if [[ "$PlatformVersionID" == 0.3* ]]; then
                __RuntimeID=ubuntu.14.04-x64
            elif [[ "$PlatformVersionID" == 0.4* ]]; then
                __RuntimeID=ubuntu.16.04-x64
            fi
            ;;
        linuxmint)
            if [[ "$PlatformVersionID" == 18* ]]; then
                __RuntimeID=ubuntu.16.04-x64
            fi
            ;;
    esac
}

# Produces project.json in the current directory
# $1 is Runtime ID
generate_project_json()
{
    if [ -z $1 ]; then
        echo "Error: project.json cannot be produced without a Runtime ID being provided."
        exit 1
    fi

    echo "{"                                                                >  project.json
    echo "   \"dependencies\": {"                                           >> project.json
    echo "       \"Microsoft.VisualStudio.clrdbg\": \"$__ClrDbgVersion\""   >> project.json
    echo "   },"                                                            >> project.json
    echo "   \"frameworks\": {"                                             >> project.json
    echo "       \"netcoreapp1.0\": {"                                      >> project.json
    echo "          \"imports\": [ \"dnxcore50\", \"portable-net45+win8\" ]" >> project.json
    echo "       }"                                                         >> project.json
    echo "   },"                                                            >> project.json
    echo "   \"runtimes\": {"                                               >> project.json
    echo "      \"$1\": {}"                                                 >> project.json
    echo "   }"                                                             >> project.json
    echo "}"                                                                >> project.json
}

# Produces NuGet.config in the current directory
generate_nuget_config()
{
    echo "<?xml version=\"1.0\" encoding=\"utf-8\"?>"                                                           >  NuGet.config
    echo "<configuration>"                                                                                      >> NuGet.config
    echo "  <packageSources>"                                                                                   >> NuGet.config
    echo "      <clear />"                                                                                      >> NuGet.config
    echo "      <add key=\"api.nuget.org\" value=\"https://api.nuget.org/v3/index.json\" />"                    >> NuGet.config
    echo "  </packageSources>"                                                                                  >> NuGet.config
    echo "</configuration>"                                                                                     >> NuGet.config
}

# Parses and populates the arguments
parse_and_get_arguments()
{
    while getopts "v:l:r:suhd" opt; do
        case $opt in
            v)
                __ClrDbgMetaVersion=$OPTARG;
                ;;
            l)
                __InstallLocation=$OPTARG
                ;;
            u)
                __RemoveExistingOnUpgrade=true
                ;;
            s)
                __SkipDownloads=true
                ;;
            d)
                __LaunchClrDbg=true
                ;;
            r)
                __RuntimeID=$OPTARG
                ;;
            h)
                print_help
                exit 1
                ;;    
            \?)
                echo "Error: Invalid Option: -$OPTARG"
                print_help
                exit 1;
                ;;
            :)
                echo "Error: Option expected for -$OPTARG"
                print_help
                exit 1
                ;;
        esac
    done
}

# Parses and populates the arguments for the legacy commandline.
get_legacy_arguments()
{
    if [ ! -z "$1" ]; then
        __ClrDbgMetaVersion=$1
    fi
    if [ ! -z "$2" ]; then
        __InstallLocation=$2
    fi
}

# Prints the arguments to stdout for the benefit of the user and does a quick sanity check.
print_and_verify_arguments()
{
    echo "Using arguments"
    echo "    Version                    : '$__ClrDbgMetaVersion'"
    echo "    Location                   : '$__InstallLocation'"
    echo "    SkipDownloads              : '$__SkipDownloads'"
    echo "    LaunchClrDbgAfter          : '$__LaunchClrDbg'"
    echo "    RemoveExistingOnUpgrade    : '$__RemoveExistingOnUpgrade'"

    if [ -z $__ClrDbgMetaVersion ]; then
        echo "Error: Version is not an optional parameter"
        exit 1
    fi

    if [[ $__ClrDbgMetaVersion = \-* ]]; then
        echo "Error: Version should not start with hyphen"
        exit 1
    fi

    if [[ $__InstallLocation = \-* ]]; then
        echo "Error: Location should not start with hyphen"
        exit 1
    fi

    if [ "$__RemoveExistingOnUpgrade" = true ]; then
        if [ "$__InstallLocation" = "$__ScriptDirectory" ]; then
            echo "Error: Cannot remove the directory which has the running script. InstallLocation: $__InstallLocation, ScriptDirectory: $__ScriptDirectory"
            exit 1
        fi
    fi
}

# Prepares installation directory.
prepare_install_location()
{
    if [ -z $__InstallLocation ]; then
        echo "Error: Install location is not set"
        exit 1
    fi

    if [ -f "$__InstallLocation" ]; then
        echo "Error: Path '$2' points to a regular file and not a directory"
        exit 1
    elif [ ! -d "$__InstallLocation" ]; then
        echo 'Info: Creating install directory'
        mkdir -p $__InstallLocation
        if [ "$?" -ne 0 ]; then
            echo "Error: Unable to create install directory: '$2'"
            exit 1
        fi
    fi   
}

# Converts relative location of the installation directory to absolute location.
convert_install_path_to_absolute()
{
    if [ -z $__InstallLocation ]; then
        __InstallLocation=$(pwd)
    else
        if [ ! -d $__InstallLocation ]; then
            prepare_install_location            
        fi

        pushd $__InstallLocation > /dev/null 2>&1
        __InstallLocation=$(pwd)
        popd > /dev/null 2>&1
    fi    
}

# Computes the CLRDBG version
set_clrdbg_version()
{    
    # This case statement is done on the lower case version of version_string
    # Add new version constants here
    # 'latest' version may be updated
    # all other version contstants i.e. 'vs2015u2' may not be updated after they are finalized
    version_string="$(echo $1 | awk '{print tolower($0)}')"
    case $version_string in
        latest)
            __ClrDbgVersion=15.0.26022.0
            ;;
        vs2015u2)
            __ClrDbgVersion=15.0.26022.0
            ;;
        *)
            simpleVersionRegex="^[0-9].*"
            if ! [[ "$1" =~ $simpleVersionRegex ]]; then
                echo "Error: '$1' does not look like a valid version number."
                exit 1
            fi
            __ClrDbgVersion=$1
            ;;
    esac
}

# Removes installation directory if remove option is specified.
process_removal()
{
    if [ "$__RemoveExistingOnUpgrade" = true ]; then
    
        if [ "$__InstallLocation" = "$HOME" ]; then
            echo "Error: Cannot remove home ( $HOME ) directory."
            exit 1
        fi

        echo "Info: Attempting to remove '$__InstallLocation'"

        if [ -d $__InstallLocation ]; then
            wcOutput=$(lsof $__InstallLocation/clrdbg | wc -l)

            if [ "$wcOutput" -gt 0 ]; then
                echo "Error: clrdbg is being used in location '$__InstallLocation'"
                exit 1
            fi

            rm -rf $__InstallLocation
            if [ "$?" -ne 0 ]; then
                echo "Error: files could not be removed from '$__InstallLocation'"
                exit 1
            fi
        fi
        echo "Info: Removed directory '$__InstallLocation'"
    fi
}

# Checks if the existing copy is the latest version.
check_latest()
{
    __SuccessFile="$__InstallLocation/success.txt" 
    if [ -f "$__SuccessFile" ]; then
        __LastInstalled=$(cat "$__SuccessFile")
        echo "Info: Last installed version of clrdbg is '$__LastInstalled'"
        if [ "$__ClrDbgVersion" = "$__LastInstalled" ]; then
            __SkipDownloads=true
            echo "Info: ClrDbg is upto date"
        else
            process_removal
        fi
    else
        echo "Info: Previous installation at "$__InstallLocation" not found"
    fi
}

download_and_extract()
{
    clrdbgZip="clrdbg-${__RuntimeID}.zip"
    target="$(echo ${__ClrDbgVersion} | tr '.' '-')"
    url="$(echo https://vsdebugger.azureedge.net/clrdbg-${target}/${clrdbgZip})"
   
    echo "Downloading ${url}"
    if ! hash unzip 2>/dev/null; then
        echo "unzip command not found. Install unzip for this script to work."
        exit 1
    fi
   
    if hash wget 2>/dev/null; then
        wget -q $url -O $clrdbgZip
    elif hash curl 2>/dev/null; then
        curl -s $url -o $clrdbgZip
    else
        echo "Install curl or wget. It is needed to download clrdbg."
        exit 1
    fi
    
    if [ $? -ne  0 ]; then
        echo "Could not download ${url}"
        exit 1;
    fi
    
    unzip -o -q $clrdbgZip
    
    if [ $? -ne  0 ]; then
        echo "Failed to unzip clrdbg"
        exit 1;
    fi
    
    chmod +x ./clrdbg
    rm $clrdbgZip
}

get_script_directory

if [ -z "$1" ]; then
    print_help
    echo "Error: Missing arguments for GetClrDbg.sh"
    exit 1
else
    parse_and_get_arguments $@

    if [ -z $__ClrDbgMetaVersion ]; then
        get_legacy_arguments $@
    fi    
fi

convert_install_path_to_absolute
print_and_verify_arguments
set_clrdbg_version "$__ClrDbgMetaVersion"
echo "Info: Using clrdbg version '$__ClrDbgVersion'"

check_latest

if [ "$__SkipDownloads" = true ]; then
    echo "Info: Skipping downloads"
else
    prepare_install_location
    pushd $__InstallLocation > /dev/null 2>&1
    if [ "$?" -ne 0 ]; then
        echo "Error: Unable to cd to install directory '$__InstallLocation'"
        exit 1
    fi

    # For the rest of this script we can assume the working directory is the install path

    if [ -z $__RuntimeID ]; then
        echo 'Info: Determining Runtime ID'
        __RuntimeID=
        get_dotnet_runtime_id
        if [ -z $__RuntimeID ]; then
            echo "Error: Unable to determine dotnet Runtime ID. Please make sure that dotnet is installed and the platform is supported. Look at https://www.microsoft.com/net/core for supported platforms. Alternatively you can specify the RuntimeID with -r switch."
            print_help
            exit 1
        fi
    fi
    
    echo "Info: Using Runtime ID '$__RuntimeID'"
    download_and_extract

    echo "$__ClrDbgVersion" > success.txt
    popd > /dev/null 2>&1

    echo "Info: Successfully installed clrdbg at '$__InstallLocation'"    
fi


if [ "$__LaunchClrDbg" = true ]; then
    # Note: The following echo is a token to indicate the clrdbg is getting launched. 
    # If you were to change or remove this echo make the necessary changes in the MIEngine
    echo "Info: Launching clrdbg"
    "$__InstallLocation/clrdbg" "--interpreter=mi"
    exit $?
fi

exit 0
