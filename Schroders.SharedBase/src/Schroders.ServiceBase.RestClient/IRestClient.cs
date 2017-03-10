using System.Threading.Tasks;

namespace Schroders.ServiceBase.RestClient
{
    public interface IRestClient
    {
        string BaseUrl { get; set; }
        Task<HttpResponse<TResponse>> Get<TResponse>(RestRequest request);
        Task<HttpResponse<TResponse>> Post<TRequest, TResponse>(RestRequest request, TRequest requestData);
    }
}