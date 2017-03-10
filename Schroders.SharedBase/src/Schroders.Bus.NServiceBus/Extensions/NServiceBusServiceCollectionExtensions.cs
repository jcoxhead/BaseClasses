namespace Schroders.Bus.NServiceBus.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    using Schroders.Bus.Core;
    using Schroders.Bus.Core.Contracts;
    using Schroders.Bus.NServiceBus;

    public static class NServiceBusServiceCollectionExtensions
    {
        public static void AddNServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<IBusHandlerProvider, DefaultBusHandlerProvider>();
            services.AddSingleton<Schroders.Bus.Core.Contracts.IBus, NServiceBus>();

            services.AddSingleton<IEndpointInstanceProvider, EndpointInstanceProvider>();
        }
    }
}
