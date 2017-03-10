
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Schroders.Bus.Core;
using Schroders.Bus.Core.Contracts;


namespace Schroders.Bus.MemoryBus
{
    public class MemoryBus : IBus
    {
        private readonly List<IBusHandler> busHandlers;

        private readonly List<BusSubscription> busSubscribers = new List<BusSubscription>();

        public MemoryBus(IBusHandlerProvider busHandlerProvider)
        {
            this.busHandlers = new List<IBusHandler>(busHandlerProvider.GetHandlers());
        }

        public void Send(BusMessage message)
        {
            var handlers = this.busHandlers.FindAll(bh => bh.CanHandleMessage(message));

            var busContext = new BusContext(this);
            handlers.ForEach(busHandler => Task.Run(() => busHandler.HandleMessage(busContext, message)));
        }

        public void Batch(IBusOperation[] operations)
        {
            throw new NotImplementedException();
        }

        public void Publish(string queueName, BusEvent message)
        {
            var subscribers = this.busSubscribers.FindAll(bs => bs.TopicName == queueName);

            var busContext = new BusContext(this);
            var busMessage = new BusMessage
            {
                Payload = message.Payload,
                TopicName = message.TopicName
            };

            subscribers.ForEach(bs => Task.Run(() => bs.Handler(busContext, busMessage)));
        }

        public BusSubscription Subscribe(string queueName, Func<BusContext, BusMessage, BusHandlerResponse> handler)
        {
            var subscription = new BusSubscription { TopicName = queueName, Handler = handler };

            this.busSubscribers.Add(subscription);

            return subscription;
        }

        public void Unsubscribe(BusSubscription subscription)
        {
            this.busSubscribers.Remove(subscription);
        }
    }
}