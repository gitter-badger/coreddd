﻿using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.Tests.TestEntities;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreIoC;
using NUnit.Framework;
using Shouldly;

namespace CoreDdd.Nhibernate.Tests.UnitOfWorks
{
    [TestFixture]
    public class when_clearing_unit_of_work
    {
        [Test]
        public void entities_are_not_persisted()
        {
            var unitOfWork = IoC.Resolve<NhibernateUnitOfWork>();
            unitOfWork.BeginTransaction();
            var testEntityRepository = new NhibernateRepository<TestEntity>(unitOfWork);
            var testEntity = new TestEntity();
            testEntityRepository.Save(testEntity);


            unitOfWork.Clear();


            testEntity = testEntityRepository.Get(testEntity.Id);

            testEntity.ShouldBeNull();

            unitOfWork.Rollback();
        }
    }
}