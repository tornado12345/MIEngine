﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MICore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MICoreUnitTests
{
    public class BasicLaunchOptionsTests
    {
        [Fact]
        public void TestLaunchOptions_Local1()
        {
            string fakeFilePath = typeof(BasicLaunchOptionsTests).Assembly.Location;
            string content = string.Concat("<LocalLaunchOptions xmlns=\"http://schemas.microsoft.com/vstudio/MDDDebuggerOptions/2014\"\n",
                "MIDebuggerPath=\"", fakeFilePath, "\"\n",
                "MIDebuggerServerAddress=\"myserverbox:345\"\n",
                "ExePath=\"", fakeFilePath, "\"\n",
                "TargetArchitecture=\"arm\"\n",
                "/>");

            var baseOptions = GetLaunchOptions(content);
            Assert.IsAssignableFrom(typeof(LocalLaunchOptions), baseOptions);
            var options = (LocalLaunchOptions)baseOptions;

            Assert.Equal(options.MIDebuggerPath, fakeFilePath);
            Assert.Equal(options.MIDebuggerServerAddress, "myserverbox:345");
            Assert.Equal(options.ExePath, fakeFilePath);
            Assert.Equal(options.TargetArchitecture, TargetArchitecture.ARM);
            Assert.True(string.IsNullOrEmpty(options.AdditionalSOLibSearchPath));
            Assert.True(string.IsNullOrEmpty(options.AbsolutePrefixSOLibSearchPath));
            Assert.Equal(options.DebuggerMIMode, MIMode.Gdb);
            Assert.Equal(options.LaunchCompleteCommand, LaunchCompleteCommand.ExecRun);
            Assert.Null(options.CustomLaunchSetupCommands);
            Assert.True(options.SetupCommands != null && options.SetupCommands.Count == 0);
            Assert.True(String.IsNullOrEmpty(options.CoreDumpPath));
            Assert.False(options.UseExternalConsole);
            Assert.False(options.IsCoreDump);
        }

        [Fact]
        public void TestLaunchOptions_Local2()
        {
            // Differences from #1: There is an empty CustomLaunchSetupCommands, and MIMode is set
            string fakeFilePath = typeof(BasicLaunchOptionsTests).Assembly.Location;
            string content = string.Concat("<LocalLaunchOptions\n",
                "MIDebuggerPath=\"", fakeFilePath, "\"\n",
                "MIDebuggerServerAddress=\"myserverbox:345\"\n",
                "ExePath=\"", fakeFilePath, "\"\n",
                "TargetArchitecture=\"arm\"\n",
                "MIMode=\"clrdbg\"",
                ">\n",
                "  <CustomLaunchSetupCommands>\n",
                "  </CustomLaunchSetupCommands>\n",
                "</LocalLaunchOptions>");

            var baseOptions = GetLaunchOptions(content);
            Assert.IsAssignableFrom(typeof(LocalLaunchOptions), baseOptions);
            var options = (LocalLaunchOptions)baseOptions;

            Assert.Equal(options.MIDebuggerPath, fakeFilePath);
            Assert.Equal(options.MIDebuggerServerAddress, "myserverbox:345");
            Assert.Equal(options.ExePath, fakeFilePath);
            Assert.Equal(options.TargetArchitecture, TargetArchitecture.ARM);
            Assert.True(string.IsNullOrEmpty(options.AdditionalSOLibSearchPath));
            Assert.True(string.IsNullOrEmpty(options.AbsolutePrefixSOLibSearchPath));
            Assert.Equal(options.DebuggerMIMode, MIMode.Clrdbg);
            Assert.Equal(options.LaunchCompleteCommand, LaunchCompleteCommand.ExecRun);
            Assert.True(options.CustomLaunchSetupCommands != null && options.CustomLaunchSetupCommands.Count == 0);
            Assert.True(options.SetupCommands != null && options.SetupCommands.Count == 0);
            Assert.True(String.IsNullOrEmpty(options.CoreDumpPath));
            Assert.False(options.UseExternalConsole);
            Assert.False(options.IsCoreDump);
        }

        [Fact]
        public void TestLaunchOptions_Local3()
        {
            // Differences from #2: required argument 'MIDebuggerPath' is missing
            string fakeFilePath = typeof(BasicLaunchOptionsTests).Assembly.Location;
            string content = string.Concat("<LocalLaunchOptions\n",
                "MIDebuggerServerAddress=\"myserverbox:345\"\n",
                "ExePath=\"", fakeFilePath, "\"\n",
                "TargetArchitecture=\"arm\"\n",
                "MIMode=\"clrdbg\"",
                ">\n",
                "  <CustomLaunchSetupCommands>\n",
                "  </CustomLaunchSetupCommands>\n",
                "</LocalLaunchOptions>");

            try
            {
                var baseOptions = GetLaunchOptions(content);
                Assert.True(false, "Code path should be unreachable");
            }
            catch (InvalidLaunchOptionsException e)
            {
                Assert.True(e.Message.Contains("MIDebuggerPath"));
            }
        }


        [Fact]
        public void TestLaunchOptions_Pipe1()
        {
            string fakeFilePath = typeof(BasicLaunchOptionsTests).Assembly.Location;
            string content = string.Concat("<PipeLaunchOptions xmlns=\"http://schemas.microsoft.com/vstudio/MDDDebuggerOptions/2014\"\n",
                "PipePath=\"", fakeFilePath, "\"\n",
                "ExePath=\"/home/user/myname/foo\" ExeArguments=\"arg1 arg2\"\n",
                "TargetArchitecture=\"AMD64\"\n",
                "WorkingDirectory=\"/home/user/myname\"\n",
                ">\n",
                "  <SetupCommands>\n",
                "    <Command>-gdb-set my-example-setting on</Command>\n",
                "  </SetupCommands>\n",
                "</PipeLaunchOptions>");

            var baseOptions = GetLaunchOptions(content);
            Assert.IsAssignableFrom(typeof(PipeLaunchOptions), baseOptions);
            var options = (PipeLaunchOptions)baseOptions;

            Assert.Equal(options.PipePath, fakeFilePath);
            Assert.Equal(options.PipeCwd, null);
            Assert.Equal(options.ExePath, "/home/user/myname/foo");
            Assert.Equal(options.ExeArguments, "arg1 arg2");
            Assert.Equal(options.TargetArchitecture, TargetArchitecture.X64);
            Assert.True(string.IsNullOrEmpty(options.AdditionalSOLibSearchPath));
            Assert.True(string.IsNullOrEmpty(options.AbsolutePrefixSOLibSearchPath));
            Assert.Equal(options.DebuggerMIMode, MIMode.Gdb);
            Assert.Equal(options.LaunchCompleteCommand, LaunchCompleteCommand.ExecRun);
            Assert.True(options.CustomLaunchSetupCommands == null);
            Assert.True(options.SetupCommands != null && options.SetupCommands.Count == 1);
            Assert.True(options.SetupCommands[0].IsMICommand);
            Assert.Equal(options.SetupCommands[0].CommandText, "-gdb-set my-example-setting on");
            Assert.True(options.SetupCommands[0].Description.Contains("gdb-set"));
            Assert.False(options.SetupCommands[0].IgnoreFailures);
            Assert.True(options.PipeEnvironment.Count() == 0);
        }

        [Fact]
        public void TestLaunchOptions_Pipe2()
        {
            // Test for:
            // MIMode=lldb
            // Having commands in CustomLaunchSetupCommands
            // Specifying the LaunchCompleteCommand
            string fakeFilePath = typeof(BasicLaunchOptionsTests).Assembly.Location;
            string content = string.Concat("<PipeLaunchOptions xmlns=\"http://schemas.microsoft.com/vstudio/MDDDebuggerOptions/2014\"\n",
                "PipePath=\"", fakeFilePath, "\"\n",
                "PipeCwd=\"/home/user/my program/src\"\n",
                "ExePath=\"/home/user/myname/foo\"\n",
                "TargetArchitecture=\"x86_64\"\n",
                "AbsolutePrefixSOLibSearchPath='/system/bin'\n",
                "AdditionalSOLibSearchPath='/a/b/c;/a/b/c'\n",
                "MIMode='lldb'\n",
                ">\n",
                "  <CustomLaunchSetupCommands>\n",
                "    <Command Description='Example description'>Example command</Command>\n",
                "  </CustomLaunchSetupCommands>\n",
                "  <LaunchCompleteCommand>None</LaunchCompleteCommand>\n",
                "  <PipeEnvironment>\n",
                "    <EnvironmentEntry Name='PipeVar1' Value='PipeValue1' />\n",
                "  </PipeEnvironment>\n",
                "</PipeLaunchOptions>");

            var baseOptions = GetLaunchOptions(content);
            Assert.IsAssignableFrom(typeof(PipeLaunchOptions), baseOptions);
            var options = (PipeLaunchOptions)baseOptions;

            Assert.Equal(options.PipePath, fakeFilePath);
            Assert.Equal(options.PipeCwd, "/home/user/my program/src");
            Assert.Equal(options.ExePath, "/home/user/myname/foo");
            Assert.Equal(options.TargetArchitecture, TargetArchitecture.X64);
            Assert.Equal(options.AbsolutePrefixSOLibSearchPath, "/system/bin");
            string[] searchPaths = options.GetSOLibSearchPath().ToArray();
            Assert.Equal(searchPaths.Length, 2);
            Assert.Equal(searchPaths[0], "/home/user/myname");
            Assert.Equal(searchPaths[1], "/a/b/c");
            Assert.Equal(options.DebuggerMIMode, MIMode.Lldb);
            Assert.True(options.SetupCommands != null && options.SetupCommands.Count == 0);
            Assert.True(options.CustomLaunchSetupCommands != null && options.CustomLaunchSetupCommands.Count == 1);
            var command = options.CustomLaunchSetupCommands[0];
            Assert.False(command.IsMICommand);
            Assert.Equal(command.CommandText, "Example command");
            Assert.Equal(command.Description, "Example description");
            Assert.Equal(options.LaunchCompleteCommand, LaunchCompleteCommand.None);
            Assert.Equal(options.PipeEnvironment.First().Name, "PipeVar1");
            Assert.Equal(options.PipeEnvironment.First().Value, "PipeValue1");
        }

        // TODO this test is broken by a bug: the assembly binder only searches the unit test
        // project's output directory for dependencies and thus won't find assemblies in the
        // package cache. This can be worked around by moving the missing assembly
        // (System.Net.Security) to that directory.
        [Fact]
        public void TestLaunchOptions_Tcp1()
        {
            // Tests for:
            // TcpLaunchOptions
            // Using CustomLaunchSetupCommands without specifying a namespace
            // Using CustomLaunchSetupCommands/LaunchCompleteCommand like an attach
            string content = @"<TcpLaunchOptions
              Hostname=""destinationComputer""
              Port=""1234""
              ExePath=""/a/b/c""
              TargetArchitecture=""ARM"">
              <CustomLaunchSetupCommands>
                <Command IgnoreFailures=""false"" Description=""Attaching to the 'foo' process"">-target-attach 1234</Command>
              </CustomLaunchSetupCommands>
              <LaunchCompleteCommand>exec-continue</LaunchCompleteCommand>
            </TcpLaunchOptions>";

            var baseOptions = GetLaunchOptions(content);
            Assert.IsAssignableFrom(typeof(TcpLaunchOptions), baseOptions);
            var options = (TcpLaunchOptions)baseOptions;

            Assert.Equal(options.ExePath, "/a/b/c");
            Assert.Equal(options.TargetArchitecture, TargetArchitecture.ARM);
            Assert.Equal(options.DebuggerMIMode, MIMode.Gdb);
            Assert.True(options.SetupCommands != null && options.SetupCommands.Count == 0);
            Assert.True(options.CustomLaunchSetupCommands != null && options.CustomLaunchSetupCommands.Count == 1);
            var command = options.CustomLaunchSetupCommands[0];
            Assert.True(command.IsMICommand);
            Assert.Equal(command.CommandText, "-target-attach 1234");
            Assert.Equal(command.Description, "Attaching to the 'foo' process");
            Assert.Equal(options.LaunchCompleteCommand, LaunchCompleteCommand.ExecContinue);
        }

        [Fact]
        public void TestLaunchOptions_Tcp2()
        {
            // Test for missing port attribute
            string content = @"<TcpLaunchOptions
              Hostname=""destinationComputer""
              ExePath=""/a/b/c""
              TargetArchitecture=""ARM""/>";

            try
            {
                GetLaunchOptions(content);
                Assert.True(false, "Should be unreachable");
            }
            catch (InvalidLaunchOptionsException e)
            {
                Assert.True(e.Message.Contains("Port"));
            }
        }

        [Fact]
        public void TestLaunchOptions_BadXml1()
        {
            // Test bad XML (extra close element)
            string content = @"<TcpLaunchOptions/></TcpLaunchOptions>";

            try
            {
                GetLaunchOptions(content);
                Assert.True(false, "Should be unreachable");
            }
            catch (InvalidLaunchOptionsException e)
            {
                Assert.True(e.Message.StartsWith("Launch options", StringComparison.Ordinal));
            }
        }

        [Fact]
        public void TestLaunchOptions_BadXml2()
        {
            // Test for missing port attribute
            string content = @"<ThisIsNotAKnownType/>";

            try
            {
                GetLaunchOptions(content);
                Assert.True(false, "Should be unreachable");
            }
            catch (InvalidLaunchOptionsException e)
            {
                Assert.True(e.Message.StartsWith("Launch options", StringComparison.Ordinal));
            }
        }

        [Fact]
        public void TestLaunchOptions_BadXml3()
        {
            // Tests for:
            // TcpLaunchOptions
            // Using CustomLaunchSetupCommands without specifying a namespace
            // Using CustomLaunchSetupCommands/LaunchCompleteCommand like an attach
            string content = @"<TcpLaunchOptions xmlns=""http://schemas.microsoft.com/ThisIsABogusNamespace""
              Hostname =""destinationComputer""
              Port=""1234""
              ExePath=""/a/b/c""
              TargetArchitecture=""ARM""/>";

            try
            {
                GetLaunchOptions(content);
                Assert.True(false, "Should be unreachable");
            }
            catch (InvalidLaunchOptionsException e)
            {
                Assert.True(e.Message.StartsWith("Launch options", StringComparison.Ordinal));
            }
        }

        /// <summary>
        /// Compilation test, do not execute.
        /// Verify that types relied upon by launcher extensions are exported by MIEngine in current build. 
        /// Don't change this test without checking with C++ team.
        /// </summary>
        public void VerifyCoreApisPresent()
        {
            LaunchOptions launchOptions = new LocalLaunchOptions("/usr/bin/gdb", "10.10.10.10:2345", new List<EnvironmentEntry>());
            launchOptions.ExePath = @"c:\users\me\myapp.out";
            launchOptions.AdditionalSOLibSearchPath = @"c:\temp;e:\foo\bar";
            launchOptions.TargetArchitecture = TargetArchitecture.ARM;
            launchOptions.WorkingDirectory = "/home/user";
            launchOptions.DebuggerMIMode = MIMode.Gdb;
            launchOptions.WaitDynamicLibLoad = false;
            launchOptions.VisualizerFile = @"c:\myproject\file.natvis";
            launchOptions.SourceMap = new ReadOnlyCollection<SourceMapEntry>(new List<SourceMapEntry>());
            Microsoft.DebugEngineHost.HostConfigurationStore configStore = null;
            IDeviceAppLauncherEventCallback eventCallback = null;
            IPlatformAppLauncher iLauncher = null;
            IPlatformAppLauncherSerializer iSerializer = null;
            iLauncher.Initialize(configStore, eventCallback);
            iLauncher.OnResume();
            iLauncher.SetLaunchOptions(string.Empty, string.Empty, string.Empty, (object)null, TargetEngine.Native);
            iLauncher.SetupForDebugging(out launchOptions);
            iLauncher.Dispose();
            XmlSerializer serializer = iSerializer.GetXmlSerializer("foobar");
        }

        private LaunchOptions GetLaunchOptions(string content)
        {
            return LaunchOptions.GetInstance(null, "bogus-exe-path", null, null, content, false, null, TargetEngine.Native, null);
        }
    }
}
