
using System.Linq;
using Schroders.Logging.Core.Constants;

namespace Schroders.Logging.Core.Extensions
{
    public static class HttpContextExtractorExtensions
    {
        public static HttpContextExtractor ExtractLoggingContext(this HttpContextExtractor extractor)
        {
            var productName = extractor.RequestContext.Request.Headers[LoggingHeaderConstants.ProductName].FirstOrDefault();
            extractor.Add(LoggingContextConstants.ProductName, productName);
            extractor.Add(LoggingContextConstants.TraceLegId, extractor.RequestContext.GetLoggingContext().TraceLegId);

            return extractor;
        }
    }
}