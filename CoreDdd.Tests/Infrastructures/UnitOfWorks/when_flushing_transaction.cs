using System.Data;
using CoreDdd.Infrastructure;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

namespace CoreDdd.Tests.Infrastructures.UnitOfWorks
{
    [TestFixture]
    public class when_flushing_transaction
    {
        private ISession _session;
        private ITransaction _transaction;

        [SetUp]
        public void Context()
        {
            _session = MockRepository.GenerateStub<ISession>();
            _transaction = MockRepository.GenerateMock<ITransaction>();
            _session.Stub(a => a.BeginTransaction(IsolationLevel.ReadCommitted)).Return(_transaction);
            
            var unitOfWork = new UnitOfWork(_session);
            unitOfWork.TransactionalFlush();
        }

        [Test]
        public void commit_was_called_on_transaction()
        {
            _transaction.AssertWasCalled(a => a.Commit());
        }
    }
}