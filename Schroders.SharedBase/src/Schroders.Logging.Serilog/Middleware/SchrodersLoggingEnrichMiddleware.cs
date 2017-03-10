using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Schroders.Logging.Core.Constants;
using Schroders.Logging.Core.Extensions;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Schroders.Logging.Serilog.Middleware
{
    public class SchrodersLoggingEnrichMiddleware
    {
        private readonly RequestDelegate next;
        public SchrodersLoggingEnrichMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var properties = GetProperties(httpContext).ToArray();

            using (LogContext.PushProperties(properties))
            {
                await next.Invoke(httpContext);
            }
        }

        private static List<ILogEventEnricher> GetProperties(HttpContext httpContext)
        {
            var properties = new List<ILogEventEnricher>();

            var productName = httpContext.Request.Headers[LoggingHeaderConstants.ProductName].FirstOrDefault();
            if (productName != null)
            {
                properties.Add(new PropertyEnricher(LoggingConstants.ProductName, productName));
            }

            if (httpContext.TraceIdentifier != null)
            {
                properties.Add(new PropertyEnricher(LoggingConstants.TraceId, httpContext.TraceIdentifier));
            }

            var traceLegId = httpContext.GetLoggingContext().TraceLegId;
            if (traceLegId != null)
            {
                properties.Add(new PropertyEnricher(LoggingConstants.TraceLegId, traceLegId));
            }

            return properties;
        }
    }
}