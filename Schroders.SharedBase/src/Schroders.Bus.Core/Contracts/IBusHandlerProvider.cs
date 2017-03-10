using System.Collections.Generic;

namespace Schroders.Bus.Core.Contracts
{
    public interface IBusHandlerProvider
    {
        IEnumerable<IBusHandler> GetHandlers();
    }
}