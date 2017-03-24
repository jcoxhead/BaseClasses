using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.PipelineActions
{
    public static class GenerateIdPipelineActionContextExtensions
    {
        public static string GetId(this IPipelineActionContext pipelineActionContext)
        {
            if (pipelineActionContext == null || !pipelineActionContext.ContainsKey(GenerateIdPipelineActionBase.GenerateIdPipelineActionContextId))
            {
                return null;
            }

            return pipelineActionContext[GenerateIdPipelineActionBase.GenerateIdPipelineActionContextId].ToString();
        }
    }
}