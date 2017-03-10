namespace Schroders.Bus.Core.Contracts
{
    using System;

    public interface IBus
    {
        void Send(BusMessage message);

        void Batch(IBusOperation[] operations);

        void Publish(string queueName, BusEvent message);

        BusSubscription Subscribe(string queueName, Func<BusContext, BusMessage, BusHandlerResponse> handler);

        void Unsubscribe(BusSubscription subscription);
    }
}