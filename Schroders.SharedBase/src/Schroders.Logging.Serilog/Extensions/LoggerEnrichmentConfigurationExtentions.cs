

using System.Diagnostics;
using System.Reflection;
using Schroders.Logging.Core.Constants;
using Serilog;
using Serilog.Configuration;

namespace Schroders.Logging.Serilog.Extensions
{
    public static class LoggerEnrichmentConfigurationExtentions
    {
        public static LoggerConfiguration WithSchrodersSettings(this LoggerEnrichmentConfiguration enrichmentConfiguration, string applicationName)
        {
            var loggerConfiguration = enrichmentConfiguration.WithProperty(LoggingConstants.ApplicationName, applicationName);

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            loggerConfiguration.Enrich.WithProperty(LoggingConstants.ApplicationVersion, version);

            var processId = Process.GetCurrentProcess().Id;
            loggerConfiguration.Enrich.WithProperty(LoggingConstants.ApplicationInstanceId, processId);

            return loggerConfiguration;
        }
    }
}