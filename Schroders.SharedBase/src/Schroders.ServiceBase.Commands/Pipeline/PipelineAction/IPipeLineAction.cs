

using System;

namespace Schroders.ServiceBase.Commands.Pipeline.PipelineAction
{
    public interface IPipelineAction<TRequest, TContext>
    {
        void Execute(TContext context, Action<TContext> next);
    }
}