using System;
using Microsoft.Extensions.Logging;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.PipelineActions
{
    public class TracePipelineAction<TRequest, TContext> : IPipelineAction<TRequest, TContext>
    {
        private readonly ILogger<TracePipelineAction<TRequest, TContext>> logger;

        public TracePipelineAction(ILogger<TracePipelineAction<TRequest, TContext>> logger)
        {
            this.logger = logger;
        }

        public void Execute(TContext context, Action<TContext> next)
        {
            using (new DisposableStopwatch(ts => this.logger.LogInformation("{@ts} elapsed decorated", ts)))
            {
                next(context);
            }
        }
    }
}
