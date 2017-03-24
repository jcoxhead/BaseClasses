
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Schroders.Logging.Core.Constants;
using Schroders.Logging.Core.Extensions;

namespace Schroders.Logging.Core.Middleware
{
    public class SchrodersLoggingMiddleware
    {
        private readonly RequestDelegate next;
        public SchrodersLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var traceLegId = httpContext.Request.Headers[LoggingHeaderConstants.TraceLegId].FirstOrDefault() ?? Guid.NewGuid().ToString();
            httpContext.GetLoggingContext().TraceLegId = traceLegId;

            await next.Invoke(httpContext);
        }
    }
}