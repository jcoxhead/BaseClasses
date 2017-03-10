
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schroders.ServiceBase.RestClient.Tests.Models;

namespace Schroders.ServiceBase.RestClient.Tests
{
    [Ignore]
    [TestClass]
    public class RestClientIntegrationTests
    {
        private const string Token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjE3NUZERUEwRERGODBFQTEwMTJGOTExNkI4OUEwMzQ5NDVDRDU1RkIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJGMV9lb04zNERxRUJMNUVXdUpvRFNVWE5WZnMifQ.eyJuYmYiOjE0ODQ1NTg0NDIsImV4cCI6MTQ4NDU1ODc0MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDA1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDA1L3Jlc291cmNlcyIsImNsaWVudF9pZCI6IlNjaHJvZGVycyBBUEkiLCJzdWIiOiJhZG1pbiIsImF1dGhfdGltZSI6MTQ4NDU1ODQ0MiwiaWRwIjoibG9jYWwiLCJzY2hyb2RlcnNfdXNlcl9pbmZvIjoie1wiVXNlck5hbWVcIjpcImFkbWluXCIsXCJEaXNwbGF5TmFtZVwiOlwiU2Nocm9kZXJzIEFkbWluXCJ9Iiwicm9sZSI6WyJhZG1pbiIsInVzZXIiXSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJzY2hyb2RlcnM6cHJvZHVjdHNfYXBpIl0sImFtciI6WyJwYXNzd29yZCJdfQ.PWPI-k-5gYTjujTsTrQmeMsF8NZV841nUF2xfr-_4uUuk9MEeCMRZrs027B8Bfe7HTE2DSxN7c75jliAw6ofwZcSu6kiGEo1hCUgzFbIwXWNWTnvQIUy7eI-WzmVAjx3nU2-r4i-Yuqk_tkuXa-2ZLaVXDWcYQ1iGvpd6g9cf3KseodPKV14basuSTt_rzeUHI6ZaviQS-DV5N6ppAy_iC4RTu6Cc2t5LoendHIsD0arF1J9qgB-9ItBaHzxjrCSXS7DQBLQA8Nc02Qs5v6Lf1MSO4wTqxLvB_Flm3HkKkGj2R92zBfI76f2b3OtocavrGSUebaAoextPq7ui6UkvQ";

        [TestMethod]
        public void RestClient_GetProducts()
        {
            var restClient = new RestClient
            {
                BaseUrl = "http://localhost:5001/products"
            };

            const string productId = "02ca9141-b010-43f7-a76f-69d9bc7a33f7";

            var request = new RestRequest()
                .AddUrlSegment(productId)
                .AddToken(Token);

            var response = restClient.Get<GetProductResponse>(request);

            var result = response.Result;
        }
    }
}