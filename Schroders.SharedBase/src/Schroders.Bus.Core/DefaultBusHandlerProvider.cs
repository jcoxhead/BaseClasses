using System.Collections.Generic;
using Schroders.Bus.Core.Contracts;

namespace Schroders.Bus.Core
{
    public class DefaultBusHandlerProvider : IBusHandlerProvider
    {
        private readonly IEnumerable<IBusHandler> handlers;

        public DefaultBusHandlerProvider(IEnumerable<IBusHandler> handlers)
        {
            this.handlers = handlers;
        }

        public IEnumerable<IBusHandler> GetHandlers()
        {
            return this.handlers;
        }
    }
}