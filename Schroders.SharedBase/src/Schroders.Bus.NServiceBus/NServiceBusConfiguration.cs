namespace Schroders.Bus.NServiceBus.Configurations
{
    using System;
    using System.Linq;

    public class NServiceBusConfiguration
    {
        public BusEndpointConfiguration[] Endpoints { get; set; }

        public BusEndpointConfiguration GetByName(string name)
        {
            var endpointConfiguration = this.Endpoints.FirstOrDefault(ep => ep.Name.Equals(name, StringComparison.InvariantCulture));
            if (endpointConfiguration == null)
            {
                throw new ArgumentOutOfRangeException(nameof(name), name, "Configuration with given name was not found. Check if it is defined in configuration");
            }

            return endpointConfiguration;
        }
    }
}