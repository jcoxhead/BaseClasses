namespace Schroders.Bus.Core
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Schroders.Bus.Core.Contracts;

    public class EmptyMessageContext : IBusMessageContext
    {
        public static readonly IBusMessageContext Instance = new EmptyMessageContext();

        public EmptyMessageContext()
        {
            this.Username = null;
            this.RequestId = null;
            this.Values = new ReadOnlyDictionary<object, object>(new Dictionary<object, object>());
        }

        public string Username { get; }

        public string RequestId { get; }

        public IDictionary<object, object> Values { get; }
    }
}