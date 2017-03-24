



using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.Pipeline.PipelineFactory
{
    public interface IPipelineFactory
    {
        IPipeline<TRequest, TResponse, PipelineActionContext> Get<TRequest, TResponse>();
    }
}