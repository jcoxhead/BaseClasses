


using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schroders.ServiceBase.ResolvableServiceClient.Tests.Models;
using Schroders.ServiceBase.RestClient;
using Schroders.ServiceDiscovery.RestClient;
using Schroders.ServiceDiscovery.RestClient.Configurations;

namespace Schroders.ServiceBase.ResolvableServiceClient.Tests
{
    [Ignore]
    [TestClass]
    public class ResolvableServiceClientIntegrationTests
    {
        private const string Token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjE3NUZERUEwRERGODBFQTEwMTJGOTExNkI4OUEwMzQ5NDVDRDU1RkIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJGMV9lb04zNERxRUJMNUVXdUpvRFNVWE5WZnMifQ.eyJuYmYiOjE0ODQyMjU3OTIsImV4cCI6MTQ4NDIyNjA5MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDA1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDA1L3Jlc291cmNlcyIsImNsaWVudF9pZCI6IlNjaHJvZGVycyBBUEkiLCJzdWIiOiJhZG1pbiIsImF1dGhfdGltZSI6MTQ4NDIyNTc5MiwiaWRwIjoibG9jYWwiLCJzY2hyb2RlcnNfdXNlcl9pbmZvIjoie1wiVXNlck5hbWVcIjpcImFkbWluXCIsXCJEaXNwbGF5TmFtZVwiOlwiU2Nocm9kZXJzIEFkbWluXCJ9Iiwicm9sZSI6WyJhZG1pbiIsInVzZXIiXSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJzY2hyb2RlcnM6cHJvZHVjdHNfYXBpIl0sImFtciI6WyJwYXNzd29yZCJdfQ.Hl9t8oMcpu2psxDlRS0jfuv9uiVgX6AY8WmGNVOxUoOSJRxfySmqK26GSiG03O3u9IW-CJOStLXC6RcxMQBLWGkZp5UQCm7kfuK6VRAmvvVuQJpMpp1K0Dk8LfgQyMmytVb5HBYuAR_Ms6PHI86jJHzWHnAP9uIT86PubjyGwruzm_Z8KNmJOt3SuqbVzfDNwJ5DczVQ9e-_8H6xiB-zMTBL-UvjJxP1lhPNgjEalGMguGT7oJhLp0vdw_txQ96mVtOBLxdZwB781PMVRVQDnU23CZOHOEf7bIsvKeUit0FyFaBa7KjWRmasnZsWZg-KxERh10Qqd6V55dELvAPewQ";

        [TestMethod]
        public void ResolvableServiceClient_GetProducts()
        {
            const string productId = "008f6f44-9cce-4358-82f0-c7a26675f2b9";

            var restClient = new RestClient.RestClient();
            var serviceDiscoveryClient = GetServiceDiscoveryClient();

            var serviceClient = new ResolvableServiceClient("products", "v1", serviceDiscoveryClient, restClient);

            var request = new RestRequest()
                .AddUrlSegment(productId)
                .AddToken(Token);

            var response = serviceClient.Get<GetProductResponse>(request);

            var result = response;
        }

        private static ServiceDiscoveryClient GetServiceDiscoveryClient()
        {
            var serviceDiscoveryConfiguration = new ServiceDiscoveryConfiguration();
            serviceDiscoveryConfiguration.ServiceUrl = "http://localhost:5000/service-discovery";
            var settings = Options.Create(serviceDiscoveryConfiguration);
            var restClient = new RestClient.RestClient();
            var serviceDiscoveryClient = new ServiceDiscoveryClient(settings);
            return serviceDiscoveryClient;
        }
    }
}