using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Schroders.ServiceBase.RestClient
{
    public class RestClient : IRestClient
    {
        public string BaseUrl { get; set; }
        private readonly IHttpClient httpClient;

        public RestClient(IHttpClient httpClient = null)
        {
            this.httpClient = httpClient ?? new HttpClient();
        }

        public async Task<HttpResponse<TResponse>> Get<TResponse>(RestRequest request)
        {
            var response = await MakeHttpRequest<TResponse>(HttpMethod.Get.Method, request);
            return response;
        }

        public async Task<HttpResponse<TResponse>> Post<TRequest, TResponse>(RestRequest request, TRequest requestData)
        {
            var response = await MakeHttpRequest<TRequest, TResponse>(HttpMethod.Post.Method, request, requestData);
            return response;
        }

        private async Task<HttpResponse<TResponse>> MakeHttpRequest<TResponse>(string method, RestRequest request)
        {
            return await MakeHttpRequest<object, TResponse>(method, request, null);
        }

        private async Task<HttpResponse<TResponse>> MakeHttpRequest<TRequest, TResponse>(string method, RestRequest request, TRequest requestData)
        {
            var requestMessage = CreateHttpRequestMessage(method, request, requestData);

            var responseMessage = await httpClient.SendAsync(requestMessage);
            responseMessage.EnsureSuccessStatusCode();

            var responseContent = ParseResponse<TResponse>(responseMessage);

            return new HttpResponse<TResponse>
            {
                Content = responseContent.Result,
                StatusCode = responseMessage.StatusCode
            };
        }

        private HttpRequestMessage CreateHttpRequestMessage<TRequest>(string method, RestRequest request, TRequest requestData)
        {
            var url = BuildUrl(BaseUrl, request.Url);

            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(url)
            };

            if (requestData != null)
            {
                var jsonString = JsonConvert.SerializeObject(request);
                requestMessage.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            }

            AddHeaders(requestMessage, request.Headers);

            return requestMessage;
        }

        private static async Task<TResponse> ParseResponse<TResponse>(HttpResponseMessage responseMessage)
        {
            var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<TResponse>(responseMessageContent);
            return responseContent;
        }

        private static void AddHeaders(HttpRequestMessage requestMessage, IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        private static string BuildUrl(string baseUrl, string requestUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl), "Service base URL is not set in rest client.");
            }

            var url = baseUrl;
            if (url.Last() == '/')
            {
                url.Remove(url.Length - 1);
            }

            url += requestUrl;
            return url;
        }
    }
}
