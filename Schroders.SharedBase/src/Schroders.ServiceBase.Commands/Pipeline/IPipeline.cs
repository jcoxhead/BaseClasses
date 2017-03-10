using System.Collections.Generic;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.Pipeline
{
    public interface IPipeline<in TRequest, TResponse, TContext>
        where TContext : IPipelineActionContext, new()
    {
        IPipelineResult<TResponse, TContext> Execute(TRequest requestParam, IDictionary<string, object> initialContext = null);
    }
}