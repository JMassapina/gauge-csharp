﻿// Copyright 2015 ThoughtWorks, Inc.
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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gauge.CSharp.Lib.Attribute;
using Gauge.CSharp.Runner.Extensions;
using Gauge.CSharp.Runner.Models;
using Moq;
using NUnit.Framework;

namespace Gauge.CSharp.Runner.UnitTests
{
    [TestFixture]
    public class HookRegistryTests
    {
        [SetUp]
        public void Setup()
        {
            _mockAssemblyScanner = new Mock<IAssemblyLoader>();
            _mockAssemblyScanner.Setup(scanner => scanner.GetTargetLibAssembly()).Returns(typeof(Step).Assembly);
            var types = new[]
            {
                typeof(BeforeScenario), typeof(AfterScenario), typeof(BeforeSpec), typeof(AfterSpec),
                typeof(BeforeStep), typeof(AfterStep), typeof(BeforeSuite), typeof(AfterSuite)
            };
            foreach (var type in types)
            {
                var methodInfos = new List<MethodInfo> {GetType().GetMethod(string.Format("{0}Hook", type.Name))};
                _mockAssemblyScanner.Setup(scanner => scanner.GetMethods(type.FullName)).Returns(methodInfos);
            }
            _hookRegistry = new HookRegistry(_mockAssemblyScanner.Object);
        }

        public void BeforeSuiteHook()
        {
        }

        public void AfterSuiteHook()
        {
        }

        public void BeforeSpecHook()
        {
        }

        public void AfterSpecHook()
        {
        }

        public void BeforeScenarioHook()
        {
        }

        public void AfterScenarioHook()
        {
        }

        public void BeforeStepHook()
        {
        }

        public void AfterStepHook()
        {
        }

        public ISandbox SandBox;
        private HookRegistry _hookRegistry;
        private Mock<IAssemblyLoader> _mockAssemblyScanner;

        [Test]
        public void ShoulddGetAfterScenarioHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("AfterScenarioHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.AfterScenarioHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetAfterSpecHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("AfterSpecHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.AfterSpecHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetAfterStepHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("AfterStepHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.AfterStepHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetAfterSuiteHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("AfterSuiteHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.AfterSuiteHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetBeforeScenarioHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("BeforeScenarioHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.BeforeScenarioHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetBeforeSpecHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("BeforeSpecHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.BeforeSpecHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetBeforeStepHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("BeforeStepHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.BeforeStepHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }

        [Test]
        public void ShouldGetBeforeSuiteHook()
        {
            var expectedMethods = new[] {GetType().GetMethod("BeforeSuiteHook").FullyQuallifiedName()};
            var hooks = _hookRegistry.BeforeSuiteHooks.Select(mi => mi.Method);

            Assert.AreEqual(expectedMethods, hooks);
        }
    }
}