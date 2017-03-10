namespace Schroders.Bus.NServiceBus.Tests
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Schroders.Bus.NServiceBus.Configurations;

    [TestClass]
    public class NServiceBusConfigurationTests
    {
        [TestMethod]
        public void GivenJsonConfigurationShouldParseCorrectly()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("TestSettings.json");

            var root = configurationBuilder.Build();
            var section = root.GetSection(nameof(NServiceBusConfiguration));

            var configuration = new NServiceBusConfiguration();
            section.Bind(configuration);

            Assert.AreEqual(2, configuration.Endpoints.Length);
        }

        [TestMethod]
        public void GivenJsonConfigurationShouldGetByName()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("TestSettings.json");

            var root = configurationBuilder.Build();
            var section = root.GetSection(nameof(NServiceBusConfiguration));

            var configuration = new NServiceBusConfiguration();
            section.Bind(configuration);

            var config = configuration.GetByName("Schroders.Product.Handler");
            Assert.IsNotNull(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenJsonConfigurationGivenBadNameShouldThrow()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("TestSettings.json");

            var root = configurationBuilder.Build();
            var section = root.GetSection(nameof(NServiceBusConfiguration));

            var configuration = new NServiceBusConfiguration();
            section.Bind(configuration);

            var config = configuration.GetByName("non existing one");
        }
    }
}