using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Schroders.ServiceBase.ResolvableServiceClient.Tests.Models;
using Schroders.ServiceBase.RestClient;
using Schroders.ServiceDiscovery.RestClient;
using Schroders.Test.Infrastructure;
using System.Threading.Tasks;

namespace Schroders.ServiceBase.ResolvableServiceClient.Tests
{
    [TestClass]
    public class ResolvableServiceClientTests : TestBase
    {
        [TestMethod]
        public void ResolvableServiceClient_Get()
        {
            var serviceDiscoveryClientMock = new Mock<IServiceDiscoveryClient>();
            serviceDiscoveryClientMock.Setup(x => x.GetServiceUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("http://localhost");

            var httpResponse = new HttpResponse<GetProductResponse>()
            {
                Content = new GetProductResponse
                {
                    Product = new ReadProduct
                    {
                        Id = "TestId"
                    }
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };

            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(x => x.Get<GetProductResponse>(It.IsAny<RestRequest>()))
                .Returns(Task.FromResult(httpResponse));

            var resolvableServiceClient = new ResolvableServiceClient(null, null, serviceDiscoveryClientMock.Object, restClientMock.Object);

            var request = new RestRequest();

            var response = resolvableServiceClient.Get<GetProductResponse>(request);
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Product);
            Assert.AreEqual(response.Content.Product.Id, "TestId");
        }
    }
}