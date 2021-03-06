﻿#if NETCOREAPP
using System;
using System.Threading.Tasks;
using CoreDdd.Domain.Events;
using CoreDdd.Domain.Repositories;
using CoreDdd.Nhibernate.Tests.TestEntities;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDdd.TestHelpers.DomainEvents;
using CoreIoC;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Shouldly;

namespace CoreDdd.Nhibernate.Tests.Webs.AspNetCoreTests.UnitOfWorkMiddlewares
{
    [TestFixture(TypeArgs = new[] { typeof(UnitOfWorkMiddlewareSpecification) })]
    [TestFixture(TypeArgs = new[] { typeof(UnitOfWorkDependencyInjectionMiddlewareSpecification) })]
    public class when_saving_entity_and_throwing_exception_within_unit_of_work<TUnitOfWorkMiddlewareSpecification>
        where TUnitOfWorkMiddlewareSpecification : IUnitOfWorkMiddlewareSpecification, new()
    {
        private IRepository<TestEntityWithDomainEvent> _entityRepository;
        private TestEntityWithDomainEvent _entity;
        private TestDomainEvent _raisedDomainEvent;

        [SetUp]
        public async Task Context()
        {
            var specification = new TUnitOfWorkMiddlewareSpecification();

            var domainEventHandlerFactory = new FakeDomainEventHandlerFactory(domainEvent => _raisedDomainEvent = (TestDomainEvent)domainEvent);
            DomainEvents.Initialize(domainEventHandlerFactory, isDelayedDomainEventHandlingEnabled: true);
            DomainEvents.ResetDelayedEventsStorage();

            async Task _requestDelegate(HttpContext context)
            {
                _entityRepository = IoC.Resolve<IRepository<TestEntityWithDomainEvent>>();
                _entity = new TestEntityWithDomainEvent();
                await _entityRepository.SaveAsync(_entity);

                throw new NotSupportedException("test exception");
            }

            try
            {
                await specification.CreateMiddlewareAndInvokeHandling(_requestDelegate);
            }
            catch (NotSupportedException) {}
        }

        [Test]
        public async Task entity_is_not_persisted_and_cannot_be_fetched_after_the_transaction_rollback()
        {
            _entity.ShouldNotBeNull();

            var unitOfWork = IoC.Resolve<NhibernateUnitOfWork>();
            unitOfWork.BeginTransaction();

            _entityRepository = IoC.Resolve<IRepository<TestEntityWithDomainEvent>>();
            _entity = await _entityRepository.GetAsync(_entity.Id);

            _entity.ShouldBeNull();

            unitOfWork.Rollback();
        }

        [Test]
        public void domain_event_is_not_handled()
        {
            _raisedDomainEvent.ShouldBeNull();
        }
    }
}
#endif