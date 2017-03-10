
namespace Schroders.Bus.NServiceBus
{
    public interface IEndpointInstanceProvider
    {
        NServiceBusInstance Get(string instanceName);
    }
}