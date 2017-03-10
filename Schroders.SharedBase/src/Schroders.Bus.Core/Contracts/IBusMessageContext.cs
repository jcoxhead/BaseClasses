namespace Schroders.Bus.Core.Contracts
{
    using System.Collections.Generic;

    public interface IBusMessageContext
    {
        string Username { get; }

        string RequestId { get; }

        IDictionary<object, object> Values { get; }
    }
}