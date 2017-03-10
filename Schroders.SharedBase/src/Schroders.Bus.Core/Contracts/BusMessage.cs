namespace Schroders.Bus.Core
{
    using Schroders.Bus.Core.Contracts;

    public class BusMessage
    {
        public BusMessage()
        {
            this.MessageContext = new BusMessageContext();
        }

        public string TopicName { get; set; }

        public object Payload { get; set; }

        public IBusMessageContext MessageContext { get; set; }
    }
}