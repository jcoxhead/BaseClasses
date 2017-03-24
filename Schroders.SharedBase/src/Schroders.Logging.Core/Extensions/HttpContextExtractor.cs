
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Schroders.Logging.Core.Extensions
{
    public class HttpContextExtractor : Dictionary<string, object>
    {
        public HttpContext RequestContext { get; }

        public HttpContextExtractor(HttpContext context)
        {
            this.RequestContext = context;
        }
    }
}