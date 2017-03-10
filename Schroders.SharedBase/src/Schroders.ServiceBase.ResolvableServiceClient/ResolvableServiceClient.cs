using System;
using Schroders.ServiceBase.RestClient;
using Schroders.ServiceDiscovery.RestClient;

namespace Schroders.ServiceBase.ResolvableServiceClient
{
    public class ResolvableServiceClient
    {
        private readonly string serviceName;
        private readonly string serviceVersion;
        private readonly IServiceDiscoveryClient serviceDiscoveryClient;
        private readonly IRestClient restClient;

        public ResolvableServiceClient(string serviceName, string serviceVersion, IServiceDiscoveryClient serviceDiscoveryClient, IRestClient restClient)
        {
            this.serviceName = serviceName;
            this.serviceVersion = serviceVersion;
            this.serviceDiscoveryClient = serviceDiscoveryClient;
            this.restClient = restClient;
        }

        public HttpResponse<TResponse> Get<TResponse>(RestRequest request)
        {
            var serviceUrl = serviceDiscoveryClient.GetServiceUrl(serviceName, serviceVersion, request.Token);
            if (serviceUrl == null)
            {
                throw new Exception($"Service {serviceVersion}/{serviceName} not found.");
            }

            restClient.BaseUrl = serviceUrl;
            var response = restClient.Get<TResponse>(request).Result;
            return response;
        }

        public HttpResponse<TResponse> Post<TRequest, TResponse>(RestRequest request, TRequest requestData)
        {
            var serviceUrl = serviceDiscoveryClient.GetServiceUrl(serviceName, serviceVersion, request.Token);
            if (serviceUrl == null)
            {
                throw new Exception($"Service {serviceVersion}/{serviceName} not found.");
            }

            restClient.BaseUrl = serviceUrl;
            var response = restClient.Post<TRequest, TResponse>(request, requestData).Result;
            return response;
        }
    }
}