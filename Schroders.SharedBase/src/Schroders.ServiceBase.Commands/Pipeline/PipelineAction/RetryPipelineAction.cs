
using System;
using Microsoft.Extensions.Logging;
using Polly;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.PipelineActions
{
    public class RetryPipelineAction<TRequest, TContext> : IPipelineAction<TRequest, TContext>
        where TContext : IPipelineActionContext
    {
        private readonly ILogger<RetryPipelineAction<TRequest, TContext>> logger;

        public RetryPipelineAction(ILogger<RetryPipelineAction<TRequest, TContext>> logger)
        {
            this.logger = logger;
        }

        public void Execute(TContext context, Action<TContext> next)
        {
            var policy = Policy.Handle<Exception>().Retry(
                2,
                (exception, retryCount) =>
                {
                    this.logger.LogWarning("Retry error handler cought: {@exception}, retry count: {@retryCount}, operation id: {@context.GetId()}", exception, retryCount, context.GetId());
                });

            var policyResult = policy.ExecuteAndCapture(() => next(context));
            if (policyResult.Outcome != OutcomeType.Failure)
            {
                return;
            }

            this.logger.LogError("Retry pipeline action failed with exception: {@policyResult.FinalException}", policyResult.FinalException);
            throw policyResult.FinalException;
        }
    }
}
