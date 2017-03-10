namespace Schroders.Bus.Core.Contracts
{
    public interface IBusOperation
    {
        BusOperationType OperationType { get; set; }

        BusMessage Message { get; set; }
    }
}