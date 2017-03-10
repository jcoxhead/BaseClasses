using Microsoft.AspNetCore.Http;
using Schroders.Logging.Core.Context;

namespace Schroders.Logging.Core.Extensions
{
    public static class HttpContextExtensions
    {
        public static LoggingContext GetLoggingContext(this HttpContext httpContext)
        {
            const string loggingContextItemName = "LoggingContext";

            if (httpContext.Items[loggingContextItemName] == null)
            {
                httpContext.Items[loggingContextItemName] = new LoggingContext();
            }

            return httpContext.Items[loggingContextItemName] as LoggingContext;
        }
    }
}
