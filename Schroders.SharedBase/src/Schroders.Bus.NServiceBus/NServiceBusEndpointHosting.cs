namespace Schroders.Bus.NServiceBus.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Autofac;

    using global::NServiceBus;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Schroders.Bus.NServiceBus.Configurations;
    using Schroders.Bus.NServiceBus.Extensions;

    public class NServiceBusEndpointHosting
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "NServiceBus again here")]
        public static void StartConfiguredEndpoints(
            IConfigurationRoot configurationRoot,
            IContainer container,
            IDictionary<string, string[]> endpointToTopicMapping,
            Action<EndpointConfiguration> allEndpointSetupOptions = null,
            IDictionary<string, Action<EndpointConfiguration>> setupEndpointOptions = null)
        {
            var endpointInstanceProvider = container.Resolve<IEndpointInstanceProvider>() as EndpointInstanceProvider;
            if (endpointInstanceProvider == null)
            {
                throw new Exception("Could not resolve IEndpointInstanceProvider from given container. Make sure its registered with AddNServiceBus or manually.");
            }

            var section = configurationRoot.GetSection(nameof(NServiceBusConfiguration));
            var nServiceBusConfiguration = new NServiceBusConfiguration();
            section.Bind(nServiceBusConfiguration);

            var busEndpointConfigurations = nServiceBusConfiguration.Endpoints;
            foreach (var busEndpointOptions in busEndpointConfigurations)
            {
                var endpointConfiguration = new EndpointConfiguration(busEndpointOptions.Name);
                endpointConfiguration.SendFailedMessagesTo(busEndpointOptions.FailedMessagesQueueName);
                endpointConfiguration.UseSerialization<JsonSerializer>();
                endpointConfiguration.EnableInstallers();
                endpointConfiguration.UsePersistence<InMemoryPersistence>();
                endpointConfiguration.UseContainer<AutofacBuilder>(
                    customizations => customizations.ExistingLifetimeScope(container));

                allEndpointSetupOptions?.Invoke(endpointConfiguration);
                if (setupEndpointOptions != null && setupEndpointOptions.ContainsKey(busEndpointOptions.Name))
                {
                    var setupFunc = setupEndpointOptions[busEndpointOptions.Name];
                    setupFunc?.Invoke(endpointConfiguration);
                }

                var endpointInstance = Endpoint.Start(endpointConfiguration).ConfigureAwait(false).GetAwaiter().GetResult();
                if (endpointToTopicMapping.ContainsKey(busEndpointOptions.Name))
                {
                    var topicNames = endpointToTopicMapping[busEndpointOptions.Name];
                    foreach (var topicName in topicNames)
                    {
                        endpointInstanceProvider.Add(topicName, busEndpointOptions.Name, endpointInstance);
                    }
                }
                else
                {
                    // Todo: throw warning that no mapping exist to an endpoint (maybe no need for handler only)
                    var loggerFactory = container.Resolve<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger(typeof(NServiceBusServiceCollectionExtensions));
                    logger.LogWarning($"Endpoint instance {busEndpointOptions.Name} does not have mapping to topics. Make sure you did not forget to add mappings.");
                }
            }
        }
    }

}