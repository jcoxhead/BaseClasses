

namespace Schroders.Bus.Core
{
    using Schroders.Bus.Core.Contracts;

    public class BusContext
    {
        public BusContext(IBus bus)
        {
            this.Bus = bus;
        }

        public IBus Bus { get; }
    }
}