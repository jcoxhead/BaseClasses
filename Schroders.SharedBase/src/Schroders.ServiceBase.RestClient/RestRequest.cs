
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schroders.ServiceBase.RestClient
{
    public class RestRequest
    {
        private readonly List<string> urlSegments;
        private readonly List<string> urlParameters;

        public string Url => BuildUrl();
        public string Token { get; private set; }

        public IDictionary<string, string> Headers { get; set; }

        public RestRequest()
        {
            urlSegments = new List<string>();
            urlParameters = new List<string>();
            Headers = new Dictionary<string, string>();
        }

        public RestRequest AddUrlParameter(string name, string value)
        {
            urlParameters.Add($"{name}={value}");
            return this;
        }

        public RestRequest AddUrlSegment(string urlSegment)
        {
            urlSegments.Add(urlSegment);
            return this;
        }

        public RestRequest AddHeaders(IDictionary<string, string> headers)
        {
            Headers = Headers.Concat(headers).ToDictionary(x => x.Key, x => x.Value);
            return this;
        }

        public RestRequest AddAuthorizationHeader(string authorizationHeader)
        {
            Headers.Add("Authorization", authorizationHeader);
            return this;
        }

        public RestRequest AddToken(string token)
        {
            this.Token = token;
            Headers.Add("Authorization", $"Bearer {token}");
            return this;
        }

        private string BuildUrl()
        {
            var url = new StringBuilder();

            urlSegments.ForEach(x => url.Append($"/{x}"));

            if (urlParameters.Any())
            {
                url.Append($"?{urlParameters.FirstOrDefault()}");

                urlParameters.Skip(1).ToList().ForEach(x => url.Append($"&{x}"));
            }

            return url.ToString();
        }
    }
}