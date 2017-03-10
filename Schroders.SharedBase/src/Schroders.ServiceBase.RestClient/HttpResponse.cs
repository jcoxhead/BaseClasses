using System.Net;

namespace Schroders.ServiceBase.RestClient
{
    public class HttpResponse<TResponse>
    {
        public TResponse Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
