using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schroders.ServiceBase.RestClient.Extensions;
using System.Linq;

namespace Schroders.ServiceBase.RestClient.Tests.Extensions
{

    [TestClass]
    public class RestClientServiceCollectionExtensionsTests
    {

        [TestMethod]
        public void AddRestClientServiceCollection_ShouldSucceed()
        {
            var services = new ServiceCollection();
            services.AddRestClient();

            Assert.IsTrue(services.Any(x => x.ImplementationType == typeof(RestClient)));
        }

    }
}