namespace Schroders.Bus.Core
{
    using System.Collections.Generic;

    using Schroders.Bus.Core.Contracts;

    public class BusMessageContext : IBusMessageContext
    {
        public BusMessageContext()
        {
            this.Values = new Dictionary<object, object>();
        }

        public string Username { get; set; }

        public string RequestId { get; set; }

        public IDictionary<object, object> Values { get; }
    }
}