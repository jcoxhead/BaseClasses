
using System;
using Microsoft.Extensions.Logging;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.PipelineActions
{
    public class GenerateIdPipelineAction<TRequest, TContext> : GenerateIdPipelineActionBase, IPipelineAction<TRequest, TContext>
        where TContext : IPipelineActionContext
    {
        private readonly ILogger<GenerateIdPipelineAction<TRequest, TContext>> logger;

        public GenerateIdPipelineAction(ILogger<GenerateIdPipelineAction<TRequest, TContext>> logger)
        {
            this.logger = logger;
        }

        public void Execute(TContext context, Action<TContext> next)
        {
            if (context != null && !context.ContainsKey(GenerateIdPipelineActionBase.GenerateIdPipelineActionContextId))
            {
                context[GenerateIdPipelineActionBase.GenerateIdPipelineActionContextId] = Guid.NewGuid();
            }

            this.logger.LogInformation("Pipeline call context: {@context}", context);
            next(context);
        }
    }
}