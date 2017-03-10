using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Schroders.Test.Infrastructure
{
    public class TestBase
    {
        protected delegate void ScopeAction(ILifetimeScope scope);

        protected delegate void RegisterTypesAction(ContainerBuilder scope);

        protected TestBase()
        {
        }

        protected ILifetimeScope CreateContainer(RegisterTypesAction builderCallback = null)
        {
            var builder = new ContainerBuilder();

            if (builderCallback != null)
            {
                builderCallback(builder);
            }

            return builder.Build();
        }

        protected void RunAction(ScopeAction action, RegisterTypesAction registerTypesAction = null)
        {
            if (action == null)
            {
                Assert.Fail("No action specified.");
            }

            using (var container = CreateContainer(registerTypesAction))
            {
                action(container);
            }
        }
    }
}