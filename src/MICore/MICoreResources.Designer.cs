﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MICore {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MICoreResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MICoreResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MICore.MICoreResources", typeof(MICoreResources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required attribute &apos;{0}&apos; is missing or has an invalid value..
        /// </summary>
        public static string Error_BadRequiredAttribute {
            get {
                return ResourceManager.GetString("Error_BadRequiredAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Both &apos;{0}&apos; and &apos;{1}&apos; cannot be specified at the same time..
        /// </summary>
        public static string Error_CannotSpecifyBoth {
            get {
                return ResourceManager.GetString("Error_CannotSpecifyBoth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command aborted. See the output window for additional details..
        /// </summary>
        public static string Error_CommandAborted {
            get {
                return ResourceManager.GetString("Error_CommandAborted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error in MIEngine. Exception of type &apos;{0}&apos; was thrown.
        ///
        ///{1}.
        /// </summary>
        public static string Error_CorruptingException {
            get {
                return ResourceManager.GetString("Error_CorruptingException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The debugger is no longer debugging the specified process..
        /// </summary>
        public static string Error_DebuggerClosed {
            get {
                return ResourceManager.GetString("Error_DebuggerClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to establish a connection to {0}. Debug output may contain more information..
        /// </summary>
        public static string Error_DebuggerInitializeFailed_NoStdErr {
            get {
                return ResourceManager.GetString("Error_DebuggerInitializeFailed_NoStdErr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to establish a connection to {0}. The following message was written to stderr:
        ///
        ///{1}
        ///
        ///See Output Window for details..
        /// </summary>
        public static string Error_DebuggerInitializeFailed_StdErr {
            get {
                return ResourceManager.GetString("Error_DebuggerInitializeFailed_StdErr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Debug server process failed to initialize..
        /// </summary>
        public static string Error_DebugServerInitializationFailed {
            get {
                return ResourceManager.GetString("Error_DebugServerInitializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PipePath cannot be empty..
        /// </summary>
        public static string Error_EmptyPipePath {
            get {
                return ResourceManager.GetString("Error_EmptyPipePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception while processing MIEngine operation. {0}. If the problem continues restart debugging..
        /// </summary>
        public static string Error_ExceptionInOperation {
            get {
                return ResourceManager.GetString("Error_ExceptionInOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while processing modules from the target process.
        ///Modules: {0}
        ///Error: {1}.
        /// </summary>
        public static string Error_ExceptionProcessingModules {
            get {
                return ResourceManager.GetString("Error_ExceptionProcessingModules", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command elements must have a body (ex: &lt;Command&gt;gdb_command_here&lt;/Command&gt;)..
        /// </summary>
        public static string Error_ExpectedCommandBody {
            get {
                return ResourceManager.GetString("Error_ExpectedCommandBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while trying to enter break state. Debugging will now stop. {0}.
        /// </summary>
        public static string Error_FailedToEnterBreakState {
            get {
                return ResourceManager.GetString("Error_FailedToEnterBreakState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal file &quot;{0}&quot; could not be found..
        /// </summary>
        public static string Error_InternalFileMissing {
            get {
                return ResourceManager.GetString("Error_InternalFileMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown launchCompleteCommand value &apos;{0}&apos;. Expected values are &apos;exec-run&apos;, &apos;exec-continue&apos; and &apos;None&apos;..
        /// </summary>
        public static string Error_InvalidLaunchCompleteCommandValue {
            get {
                return ResourceManager.GetString("Error_InvalidLaunchCompleteCommandValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Launch options string provided by the project system is invalid. {0}.
        /// </summary>
        public static string Error_InvalidLaunchOptions {
            get {
                return ResourceManager.GetString("Error_InvalidLaunchOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid core dump file path: &apos;{0}&apos;. File must be a valid file name that exists on the computer..
        /// </summary>
        public static string Error_InvalidLocalCoreDumpPath {
            get {
                return ResourceManager.GetString("Error_InvalidLocalCoreDumpPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid directory path: &apos;{0}&apos;. Directory must be a valid directory name that exists..
        /// </summary>
        public static string Error_InvalidLocalDirectoryPath {
            get {
                return ResourceManager.GetString("Error_InvalidLocalDirectoryPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid executable file path: &apos;{0}&apos;. File must be a valid file name that exists..
        /// </summary>
        public static string Error_InvalidLocalExePath {
            get {
                return ResourceManager.GetString("Error_InvalidLocalExePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value of miDebuggerPath is invalid.
        /// </summary>
        public static string Error_InvalidMiDebuggerPath {
            get {
                return ResourceManager.GetString("Error_InvalidMiDebuggerPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid SymbolInfo, cannot specify a list of solibs when &quot;WaitDynamicLoad&quot; is false..
        /// </summary>
        public static string Error_InvalidSymbolInfo {
            get {
                return ResourceManager.GetString("Error_InvalidSymbolInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Device App Launcher {0} could not be found..
        /// </summary>
        public static string Error_LauncherNotFound {
            get {
                return ResourceManager.GetString("Error_LauncherNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Device App Launcher Serializer {0} could not be found..
        /// </summary>
        public static string Error_LauncherSerializerNotFound {
            get {
                return ResourceManager.GetString("Error_LauncherSerializerNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to initialize debugger terminal..
        /// </summary>
        public static string Error_LocalUnixTerminalDebuggerInitializationFailed {
            get {
                return ResourceManager.GetString("Error_LocalUnixTerminalDebuggerInitializationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} exited unexpectedly..
        /// </summary>
        public static string Error_MIDebuggerExited_UnknownCode {
            get {
                return ResourceManager.GetString("Error_MIDebuggerExited_UnknownCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} exited unexpectedly with exit code {1}..
        /// </summary>
        public static string Error_MIDebuggerExited_WithCode {
            get {
                return ResourceManager.GetString("Error_MIDebuggerExited_WithCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required attribute &apos;{0}&apos; is missing..
        /// </summary>
        public static string Error_MissingAttribute {
            get {
                return ResourceManager.GetString("Error_MissingAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to determine path to debugger.  Please specify the &quot;MIDebuggerPath&quot; option..
        /// </summary>
        public static string Error_NoMiDebuggerPath {
            get {
                return ResourceManager.GetString("Error_NoMiDebuggerPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to execute command. The MIEngine is not currently debugging any process..
        /// </summary>
        public static string Error_NoMIDebuggerProcess {
            get {
                return ResourceManager.GetString("Error_NoMIDebuggerProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No terminal is available to launch the debugger.  Please install Gnome Terminal or XTerm..
        /// </summary>
        public static string Error_NoTerminalAvailable_Linux {
            get {
                return ResourceManager.GetString("Error_NoTerminalAvailable_Linux", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: Option {0} is not supported by {1}..
        /// </summary>
        public static string Error_OptionNotSupported {
            get {
                return ResourceManager.GetString("Error_OptionNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PipeProgram &apos;{0}&apos; could not be found..
        /// </summary>
        public static string Error_PipeProgramNotFound {
            get {
                return ResourceManager.GetString("Error_PipeProgramNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an error starting the pipe program &apos;{0}&apos;. {1}.
        /// </summary>
        public static string Error_PipeProgramStart {
            get {
                return ResourceManager.GetString("Error_PipeProgramStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while processing {0}: {1}.
        /// </summary>
        public static string Error_ProcessingFile {
            get {
                return ResourceManager.GetString("Error_ProcessingFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Commands are only accepted when the process is stopped..
        /// </summary>
        public static string Error_ProcessMustBeStopped {
            get {
                return ResourceManager.GetString("Error_ProcessMustBeStopped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Property &apos;{0}&apos; cannot be modified after initialization is complete..
        /// </summary>
        public static string Error_PropertyCannotBeModifiedAfterInitialization {
            get {
                return ResourceManager.GetString("Error_PropertyCannotBeModifiedAfterInitialization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized format of field &quot;{0}&quot; in result: {1}.
        /// </summary>
        public static string Error_ResultFormat {
            get {
                return ResourceManager.GetString("Error_ResultFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;RunInTerminalRequest&apos; failed. &apos;{0}&apos;..
        /// </summary>
        public static string Error_RunInTerminalFailure {
            get {
                return ResourceManager.GetString("Error_RunInTerminalFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to send &apos;RunInTerminalRequest&apos; to Visual Studio Code..
        /// </summary>
        public static string Error_RunInTerminalUnavailable {
            get {
                return ResourceManager.GetString("Error_RunInTerminalUnavailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The format of source file map entry &apos;{0}&apos; is incorrect.  .
        /// </summary>
        public static string Error_SourceFileMapFormat {
            get {
                return ResourceManager.GetString("Error_SourceFileMapFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This string is null or empty..
        /// </summary>
        public static string Error_StringIsNullOrEmpty {
            get {
                return ResourceManager.GetString("Error_StringIsNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The target process has exited..
        /// </summary>
        public static string Error_TargetProcessExited {
            get {
                return ResourceManager.GetString("Error_TargetProcessExited", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timeout waiting for connection..
        /// </summary>
        public static string Error_TimeoutWaitingForConnection {
            get {
                return ResourceManager.GetString("Error_TimeoutWaitingForConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to establish a connection to the launcher..
        /// </summary>
        public static string Error_UnableToEstablishConnectionToLauncher {
            get {
                return ResourceManager.GetString("Error_UnableToEstablishConnectionToLauncher", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error. Failed to load serializer for type &apos;{0}&apos;..
        /// </summary>
        public static string Error_UnableToLoadSerializer {
            get {
                return ResourceManager.GetString("Error_UnableToLoadSerializer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to start debugging. {0}.
        /// </summary>
        public static string Error_UnableToStartDebugging {
            get {
                return ResourceManager.GetString("Error_UnableToStartDebugging", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected {0} output from command &quot;{1}&quot;..
        /// </summary>
        public static string Error_UnexpectedMIOutput {
            get {
                return ResourceManager.GetString("Error_UnexpectedMIOutput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected ResultClass from MI Debugger. Expected &apos;{0}&apos; but received &apos;{1}&apos;..
        /// </summary>
        public static string Error_UnexpectedResultClass {
            get {
                return ResourceManager.GetString("Error_UnexpectedResultClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized json customLauncher element &apos;{0}&apos;..
        /// </summary>
        public static string Error_UnknownCustomLauncher {
            get {
                return ResourceManager.GetString("Error_UnknownCustomLauncher", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Internal error: unable to serialize launch options..
        /// </summary>
        public static string Error_UnknownLaunchOptions {
            get {
                return ResourceManager.GetString("Error_UnknownLaunchOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown or unsupported target architecture &apos;{0}&apos;..
        /// </summary>
        public static string Error_UnknownTargetArchitecture {
            get {
                return ResourceManager.GetString("Error_UnknownTargetArchitecture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized XML element &apos;{0}&apos;..
        /// </summary>
        public static string Error_UnknownXmlElement {
            get {
                return ResourceManager.GetString("Error_UnknownXmlElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GDB version {0} is not supported on Windows. Consider upgrading.
        ///
        ///{1}.
        /// </summary>
        public static string Error_UnsupportedWindowsGdb {
            get {
                return ResourceManager.GetString("Error_UnsupportedWindowsGdb", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Installing debugger on the remote machine..
        /// </summary>
        public static string Info_InstallingDebuggerOnRemote {
            get {
                return ResourceManager.GetString("Info_InstallingDebuggerOnRemote", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Killing pipe process.
        /// </summary>
        public static string Info_KillingPipeProcess {
            get {
                return ResourceManager.GetString("Info_KillingPipeProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting unix command: &apos;{0}&apos;.
        /// </summary>
        public static string Info_StartingUnixCommand {
            get {
                return ResourceManager.GetString("Info_StartingUnixCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Debugger was unable to continue the process..
        /// </summary>
        public static string Info_UnableToContinue {
            get {
                return ResourceManager.GetString("Info_UnableToContinue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Module containing this breakpoint has not yet loaded or the breakpoint address could not be obtained..
        /// </summary>
        public static string Status_BreakpointPending {
            get {
                return ResourceManager.GetString("Status_BreakpointPending", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Superuser access is required to attach to a process. Attaching as superuser can potentially harm your computer. Do you want to continue? [y/N].
        /// </summary>
        public static string Warn_AttachAsRootProcess {
            get {
                return ResourceManager.GetString("Warn_AttachAsRootProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempt to execute {0} failed with exception {1}.
        /// </summary>
        public static string Warn_ProcessException {
            get {
                return ResourceManager.GetString("Warn_ProcessException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} exited with exit code {1}.
        /// </summary>
        public static string Warn_ProcessExit {
            get {
                return ResourceManager.GetString("Warn_ProcessExit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning: Downloading .NET debugger to remote machine failed with error {0}..
        /// </summary>
        public static string Warning_DownloadingClrDbgToRemote {
            get {
                return ResourceManager.GetString("Warning_DownloadingClrDbgToRemote", resourceCulture);
            }
        }
    }
}
