﻿using NUnit.Framework;
using Shouldly;

namespace CoreDdd.Nhibernate.PersistenceTests
{
    [TestFixture]
    public class when_comparing_derived_entity_with_parent_entity_proxy : BasePersistenceTestWithDatabaseCreation
    {
        [Test]
        public void derived_entity_and_its_parent_entity_proxy_are_equal()
        {
            var derivedEntity = new DerivedTestEntityOne();

            Save(derivedEntity);
            Clear();

            var parentEntityProxy = Load<TestEntityOne>(derivedEntity.Id);

            (derivedEntity == parentEntityProxy).ShouldBeTrue();
        }
    }
}