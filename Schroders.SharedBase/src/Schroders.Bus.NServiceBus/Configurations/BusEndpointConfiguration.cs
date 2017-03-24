namespace Schroders.Bus.NServiceBus.Configurations
{
    using System;

    public class BusEndpointConfiguration
    {
        public string Name { get; set; }

        public string FailedMessagesQueueName { get; set; }
    }
}
