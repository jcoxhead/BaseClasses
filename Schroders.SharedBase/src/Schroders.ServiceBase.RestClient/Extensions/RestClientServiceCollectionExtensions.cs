using Microsoft.Extensions.DependencyInjection;

namespace Schroders.ServiceBase.RestClient.Extensions
{
    public static class RestClientServiceCollectionExtensions
    {
        public static void AddRestClient(this IServiceCollection services)
        {
            services.AddSingleton<IRestClient, RestClient>();
        }
    }
}