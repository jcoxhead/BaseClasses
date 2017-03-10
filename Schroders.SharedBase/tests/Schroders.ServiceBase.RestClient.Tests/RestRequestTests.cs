
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Schroders.ServiceBase.RestClient.Tests
{
    [TestClass]
    public class RestRequestTests
    {
        [TestMethod]
        public void RestRequest_AddUrlParameter_ShouldSucceed()
        {
            var request = new RestRequest();

            request.AddUrlParameter("name", "value");
            Assert.AreEqual("?name=value", request.Url);

            request.AddUrlParameter("name1", "value1");
            Assert.AreEqual("?name=value&name1=value1", request.Url);
        }

        [TestMethod]
        public void RestRequest_AddUrlSegment_ShouldSucceed()
        {
            var request = new RestRequest();

            request.AddUrlSegment("value1");
            Assert.AreEqual("/value1", request.Url);
            request.AddUrlSegment("value2");
            Assert.AreEqual("/value1/value2", request.Url);
        }

    }
}