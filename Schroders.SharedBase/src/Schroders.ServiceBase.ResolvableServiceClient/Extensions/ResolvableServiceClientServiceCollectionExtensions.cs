using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Schroders.ServiceDiscovery.RestClient.Extensions;

namespace Schroders.ServiceBase.ResolvableServiceClient.Extensions
{
    public static class ResolvableServiceClientServiceCollectionExtensions
    {
        public static void AddResolvableServiceClient(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddServiceDiscoveryRestClient(configuration);
        }
    }
}

