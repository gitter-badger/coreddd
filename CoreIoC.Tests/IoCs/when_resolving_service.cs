﻿using System;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace CoreIoC.Tests.IoCs
{
    [TestFixture]
    public class when_resolving_service : IoCSetup
    {
        private interface IServiceType {}
        private class ServiceType : IServiceType { }
        
        private object _result;
        private ServiceType _serviceType;
        private Type _iServiceType;

        [SetUp]
        public override void Context()
        {
            base.Context();
            _serviceType = new ServiceType();
            _iServiceType = typeof(IServiceType);
            Container.Stub(x => x.Resolve(_iServiceType)).Return(_serviceType);

            _result = IoC.Resolve(_iServiceType);
        }

        [Test]
        public void container_is_correctly_set()
        {
            _result.ShouldBe(_serviceType);
        }
    }
}