
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Schroders.ServiceBase.RestClient.Tests.Models;

namespace Schroders.ServiceBase.RestClient.Tests
{
    [TestClass]
    public class RestClientTests
    {
        [TestMethod]
        public void RestClient_Get()
        {
            var responseObject = new GetProductResponse
            {
                Product = new ReadProduct
                {
                    Id = "TestId"
                }
            };
            var responseJsonString = JsonConvert.SerializeObject(responseObject);

            var httpClientMock = new Mock<IHttpClient>();
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(responseJsonString)
            };

            httpClientMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Returns(Task.FromResult(httpResponseMessage));

            var restClient = new RestClient(httpClientMock.Object)
            {
                BaseUrl = "http://localhost"
            };

            var request = new RestRequest().AddToken("test");

            var response = restClient.Get<GetProductResponse>(request);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
            Assert.AreEqual(response.Result.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsNotNull(response.Result.Content);
            Assert.IsNotNull(response.Result.Content.Product);
            Assert.AreEqual(response.Result.Content.Product.Id, "TestId");
        }

        [TestMethod]
        public void RestClient_Post_SchouldSucceed()
        {
            const string id = "12345";
            var product = new ReadProduct
            {
                Id = id
            };
            var responseJsonString = JsonConvert.SerializeObject(product);

            var httpClientMock = new Mock<IHttpClient>();
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Accepted)
            {
                Content = new StringContent(responseJsonString)
            };

            httpClientMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Returns(Task.FromResult(httpResponseMessage));

            var restClient = new RestClient(httpClientMock.Object)
            {
                BaseUrl = "http://localhost/"
            };

            var headers = new Dictionary<string, string>();
            headers.Add("Tracing", "id-123");
            var request = new RestRequest()
                .AddHeaders(headers)
                .AddAuthorizationHeader(Guid.NewGuid().ToString());


            var response = restClient.Post<ReadProduct, ReadProduct>(request, product).Result;

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Accepted);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual(id, response.Content.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task RestClient_BaseUrlNotSet_ShouldThrowException()
        {
            var httpClientMock = new Mock<IHttpClient>();
            var restClient = new RestClient(httpClientMock.Object);
            await restClient.Get<string>(new RestRequest());
        }
    }
}