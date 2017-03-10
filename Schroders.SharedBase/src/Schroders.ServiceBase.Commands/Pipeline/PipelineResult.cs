namespace Schroders.ServiceBase.Commands.Pipeline
{
    public class PipelineResult<TResponse, TContext> : IPipelineResult<TResponse, TContext>
    {
        private PipelineResult()
        {
        }

        public bool Success { get; private set; }

        public TResponse Result { get; private set; }

        public TContext Context { get; private set; }

        public ErrorResult Error { get; private set; }

        public static PipelineResult<TResponse, TContext> CreateSuccess(TResponse response, TContext context)
        {
            return new PipelineResult<TResponse, TContext> { Success = true, Result = response, Context = context };
        }

        public static PipelineResult<TResponse, TContext> CreateFailure(ErrorResult errorResult)
        {
            return new PipelineResult<TResponse, TContext> { Success = false, Error = errorResult };
        }

        public static PipelineResult<TResponse, TContext> CreateFailure(ErrorResult errorResult, TResponse response, TContext context)
        {
            return new PipelineResult<TResponse, TContext> { Success = false, Result = response, Context = context, Error = errorResult };
        }
    }
}