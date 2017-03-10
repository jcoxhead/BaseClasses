

namespace Schroders.Bus.NServiceBus
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using global::NServiceBus;

    public class EndpointInstanceProvider : IEndpointInstanceProvider, IEnumerable, IDisposable
    {
        private readonly IDictionary<string, NServiceBusInstance> topicToInstanceMapping = new Dictionary<string, NServiceBusInstance>();

        public NServiceBusInstance Get(string instanceName)
        {
            return this.topicToInstanceMapping.ContainsKey(instanceName) ? this.topicToInstanceMapping[instanceName] : null;
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public void Add(string topic, string endpointName, IEndpointInstance endpointInstance)
        {
            this.topicToInstanceMapping[topic] = new NServiceBusInstance(endpointName, endpointInstance);
        }

        public void Dispose()
        {
            foreach (var endpoint in this.topicToInstanceMapping.Values)
            {
                endpoint.Instance.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}