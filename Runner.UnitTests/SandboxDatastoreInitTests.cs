// Copyright 2015 ThoughtWorks, Inc.
//
// This file is part of Gauge-CSharp.
//
// Gauge-CSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Gauge-CSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Gauge-CSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Gauge.CSharp.Runner.Models;
using Gauge.CSharp.Runner.Wrappers;
using Moq;
using NUnit.Framework;

namespace Gauge.CSharp.Runner.UnitTests
{
    public class SandboxDatastoreInitTests
    {
        private string _gaugeProjectRootEnv;

        private static string[] DataStores => new[] {"Scenario", "Suite", "Spec"};

        // Can't mock Type using Moq.
        // Something like this does not work"
        //    var mockDataStoreType = new Mock<Type>();
        //    mockDataStoreType.Setup(type => type.GetMethod(string.Format("Initialize{0}DataStore", "Scenario")))
        // Throws System.NotSupportedException : Invalid setup on a non-virtual (overridable in VB) member: type => type.GetMethod(String.Format("Initialize{0}DataStore", "Scenario"))
        // HACK: simulate the methods in test type. InitDataStore methods are Static.
        // Everymethod invoke sets a global value, that can be asserted on.
        private static string InitializedDataStore { get; set; }

        [SetUp]
        public void Setup()
        {
            _gaugeProjectRootEnv = Environment.GetEnvironmentVariable("GAUGE_PROJECT_ROOT");
            Environment.SetEnvironmentVariable("GAUGE_PROJECT_ROOT", Directory.GetCurrentDirectory());
        }

        [Test]
        [TestCaseSource("DataStores")]
        public void ShouldInitializeDatastore(string dataStoreType)
        {
            var mockAssemblyLoader = new Mock<IAssemblyLoader>();
            var mockAssembly = new Mock<TestAssembly>();
            var mockLibAssembly = new Mock<TestAssembly>();
            mockAssemblyLoader.Setup(loader => loader.AssembliesReferencingGaugeLib)
                .Returns(new List<Assembly> {mockAssembly.Object});
            mockAssemblyLoader.Setup(loader => loader.ScreengrabberTypes).Returns(new List<Type>());
            mockAssemblyLoader.Setup(loader => loader.ClassInstanceManagerTypes).Returns(new List<Type>());
            mockLibAssembly.Setup(assembly => assembly.GetType("Gauge.CSharp.Lib.DataStoreFactory")).Returns(GetType());
            mockAssemblyLoader.Setup(loader => loader.GetTargetLibAssembly()).Returns(mockLibAssembly.Object);
            var mockHookRegistry = new Mock<IHookRegistry>();
            var mockFileWrapper = new Mock<IFileWrapper>();
            var sandbox = new Sandbox(mockAssemblyLoader.Object, mockHookRegistry.Object, mockFileWrapper.Object);
            InitializedDataStore = string.Empty;

            sandbox.InitializeDataStore(dataStoreType);

            Assert.AreEqual(InitializedDataStore, dataStoreType);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("GAUGE_PROJECT_ROOT", _gaugeProjectRootEnv);
        }

        public static void InitializeScenarioDataStore()
        {
            InitializedDataStore = "Scenario";
        }

        public static void InitializeSpecDataStore()
        {
            InitializedDataStore = "Spec";
        }

        public static void InitializeSuiteDataStore()
        {
            InitializedDataStore = "Suite";
        }
    }
}