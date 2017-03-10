namespace Schroders.Bus.Core.Contracts
{
    public interface IBusHandler
    {
        bool CanHandleMessage(BusMessage message);

        BusHandlerResponse HandleMessage(BusContext busContext, BusMessage busMessage);
    }
}