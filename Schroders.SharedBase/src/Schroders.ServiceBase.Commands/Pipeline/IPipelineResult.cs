
namespace Schroders.ServiceBase.Commands.Pipeline
{
    public interface IPipelineResult<TResponse, TContext>
    {
        bool Success { get; }

        TResponse Result { get; }

        TContext Context { get; }

        ErrorResult Error { get; }
    }
}