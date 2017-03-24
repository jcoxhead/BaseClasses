

using Microsoft.AspNetCore.Hosting;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Schroders.ServiceBase.Hosting
{
    public abstract class HostableService : ServiceControl
    {
        private IWebHost host;

        private readonly HostingConfiguration config;

        private readonly string contentRoot;

        public string ApplicationName => config.ApplicationName;

        protected HostableService(HostingConfiguration config, string contentRoot)
        {
            this.config = config;
            this.contentRoot = contentRoot;
        }

        protected abstract IWebHost CreateHost(HostingConfiguration config, string contentRoot);

        public abstract void UpdateHostConfigurator(HostConfigurator cfg);

        public bool Start(HostControl hostControl)
        {
            this.host = CreateHost(this.config, this.contentRoot);

            this.host.Start();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            this.host?.Dispose();

            return true;
        }
    }
}