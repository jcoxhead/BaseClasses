namespace Schroders.Bus.Core
{
    public class BusEvent
    {
        public string TopicName { get; set; }

        public object Payload { get; set; }
    }
}