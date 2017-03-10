

using Autofac;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.Pipeline.PipelineFactory
{
    public class PipelineFactory : IPipelineFactory
    {
        private readonly IComponentContext container;

        public PipelineFactory(IComponentContext container)
        {
            this.container = container;
        }

        public IPipeline<TRequest, TResponse, PipelineActionContext> Get<TRequest, TResponse>()
        {
            var pipeline = this.container.Resolve<IPipeline<TRequest, TResponse, PipelineActionContext>>();

            return pipeline;
        }
    }
}