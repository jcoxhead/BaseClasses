using System;
using Topshelf;

namespace Schroders.ServiceBase.Hosting
{
    public static class HostingFacade
    {
        public static void Run<TService>(Func<TService> serviceCreator) where TService : HostableService
        {
            HostFactory.Run(cfg =>
            {
                var service = serviceCreator();
                cfg.Service(() => service);

                cfg.SetServiceName(service.ApplicationName);

                service.UpdateHostConfigurator(cfg);
            });
        }
    }
}