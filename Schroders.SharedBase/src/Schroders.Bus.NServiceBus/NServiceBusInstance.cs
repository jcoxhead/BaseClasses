namespace Schroders.Bus.NServiceBus
{
    using System;

    using global::NServiceBus;

    public class NServiceBusInstance
    {
        public NServiceBusInstance(string name, IEndpointInstance instance)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.Name = name;
            this.Instance = instance;
        }

        public string Name { get; }

        public IEndpointInstance Instance { get; }
    }
}