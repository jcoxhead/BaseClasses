using System;
using System.Collections.Generic;
using Schroders.Logging.Core.Constants;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;
using Schroders.ServiceBase.Commands.PipelineActions;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Schroders.ServiceBase.Commands.Serilog.PipelineActions
{
    public class LoggingPipelineAction<TRequest, TContext> : IPipelineAction<TRequest, TContext>
        where TContext : IPipelineActionContext
    {
        public void Execute(TContext context, Action<TContext> next)
        {
            var properties = GetProperties(context).ToArray();

            using (LogContext.PushProperties(properties))
            {
                next(context);
            }
        }

        private static List<ILogEventEnricher> GetProperties(TContext context)
        {
            var properties = new List<ILogEventEnricher>();

            var productName = context[LoggingContextConstants.ProductName];
            if (productName != null)
            {
                properties.Add(new PropertyEnricher(LoggingConstants.ProductName, productName));
            }

            var traceLegId = context[LoggingContextConstants.TraceLegId];
            if (traceLegId != null)
            {
                properties.Add(new PropertyEnricher(LoggingConstants.TraceLegId, traceLegId));
            }

            var traceId = context[GenerateIdPipelineActionBase.GenerateIdPipelineActionContextId];
            if (traceId != null)
            {
                properties.Add(new PropertyEnricher(LoggingConstants.TraceId, traceId));
            }

            return properties;
        }
    }
}