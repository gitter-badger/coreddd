using CoreDdd.Domain.Events;
using CoreUtils.Storages;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace CoreDdd.Register.Ninject
{
    public class DomainEventsBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDomainEventHandlerFactory>().ToFactory();
            Bind<IStorageFactory>().ToFactory();
        }
    }
}