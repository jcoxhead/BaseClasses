
using System;
using Microsoft.Extensions.Logging;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.PipelineActions
{
    public class ErrorHandlingPipelineAction<TRequest, TContext> : IPipelineAction<TRequest, TContext>
        where TContext : IPipelineActionContext
    {
        private readonly ILogger<ErrorHandlingPipelineAction<TRequest, TContext>> logger;

        public ErrorHandlingPipelineAction(ILogger<ErrorHandlingPipelineAction<TRequest, TContext>> logger)
        {
            this.logger = logger;
        }

        public void Execute(TContext context, Action<TContext> next)
        {
            try
            {
                next(context);
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    context.Exception = e;
                }

                this.logger.LogError("Unhandled exception cought: {@e}, context: {@context}", e, context);
            }
        }
    }
}
