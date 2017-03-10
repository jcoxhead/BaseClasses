namespace Schroders.Bus.NServiceBus
{
    using System;

    using global::NServiceBus;

    using Schroders.Bus.Core;
    using Schroders.Bus.Core.Contracts;

    using IBus = Schroders.Bus.Core.Contracts.IBus;

    public class NServiceBus : IBus
    {
        private readonly IEndpointInstanceProvider endpointInstanceProvider;

        public NServiceBus(IEndpointInstanceProvider endpointInstanceProvider)
        {
            this.endpointInstanceProvider = endpointInstanceProvider;
        }

        public void Send(BusMessage message)
        {
            var endpoint = this.endpointInstanceProvider.Get(message.TopicName);
            if (endpoint == null)
            {
                throw new Exception($"Failed to resolve NServiceBus endpoint mapping to send topic {message.TopicName}");
            }

            endpoint.Instance.Send(endpoint.Name, message);
        }

        public void Batch(IBusOperation[] operations)
        {
            throw new NotImplementedException();
        }

        public void Publish(string queueName, BusEvent message)
        {
            var endpoint = this.endpointInstanceProvider.Get(queueName);
            if (endpoint == null)
            {
                throw new Exception($"Failed to resolve NServiceBus endpoint mapping to publishing topic {message.TopicName}");
            }

            endpoint.Instance.Publish(message);
        }

        public BusSubscription Subscribe(string queueName, Func<BusContext, BusMessage, BusHandlerResponse> handler)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(BusSubscription subscription)
        {
            throw new NotImplementedException();
        }
    }
}