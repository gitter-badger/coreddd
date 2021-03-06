﻿using System.Collections.Generic;
using CoreDdd.Nhibernate.Queries;
using CoreDdd.Nhibernate.Tests.TestEntities;
using CoreDdd.Nhibernate.UnitOfWorks;
using NHibernate.Criterion;
#if !NET40 && !NET45 && !NET451
using System.Threading.Tasks;
#endif

namespace CoreDdd.Nhibernate.Tests.Queries
{
    public class GetTestEntityCountTestNhibernateQueryHandler : BaseNhibernateQueryHandler<GetTestEntityCountTestNhibernateQuery>
    {
        public GetTestEntityCountTestNhibernateQueryHandler(NhibernateUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public override IEnumerable<TResult> Execute<TResult>(GetTestEntityCountTestNhibernateQuery query)
        {
            return Session.CreateCriteria<TestEntity>()
                .SetProjection(Projections.Count(Projections.Id()))
                .Future<TResult>();
        }

#if !NET40 && !NET45 && !NET451
        public override Task<IEnumerable<TResult>> ExecuteAsync<TResult>(GetTestEntityCountTestNhibernateQuery query)
        {
            return Session.CreateCriteria<TestEntity>()
                .SetProjection(Projections.Count(Projections.Id()))
                .Future<TResult>().GetEnumerableAsync();
        }
#endif
    }
}