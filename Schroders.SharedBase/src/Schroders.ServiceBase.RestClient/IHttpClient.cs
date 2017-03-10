
using System.Net.Http;
using System.Threading.Tasks;

namespace Schroders.ServiceBase.RestClient
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}