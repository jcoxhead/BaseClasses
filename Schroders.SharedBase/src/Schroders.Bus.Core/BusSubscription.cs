using System;

namespace Schroders.Bus.Core
{
    public class BusSubscription
    {
        public string TopicName { get; set; }

        public Func<BusContext, BusMessage, BusHandlerResponse> Handler { get; set; }
    }
}