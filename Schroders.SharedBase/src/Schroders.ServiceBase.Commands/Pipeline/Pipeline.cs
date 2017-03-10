using System;
using System.Collections.Generic;
using System.Linq;
using Schroders.ServiceBase.Commands.Pipeline.PipelineAction;

namespace Schroders.ServiceBase.Commands.Pipeline
{
    public class Pipeline<TRequest, TResponse, TContext> : IPipeline<TRequest, TResponse, TContext>
        where TContext : IPipelineActionContext, new()
    {
        private readonly IPipelineAction<TRequest, TContext>[] prePostActions;
        private readonly ICommand<TRequest, TResponse> commandAction;
        private TRequest request;

        public Pipeline(IPipelineAction<TRequest, TContext>[] prePostActions,
            ICommand<TRequest, TResponse> commandAction)
        {
            this.prePostActions = prePostActions;
            this.commandAction = commandAction;
        }

        public virtual IPipelineResult<TResponse, TContext> Execute(TRequest requestParam,
            IDictionary<string, object> initialContext = null)
        {
            var context = this.InitializeContext(initialContext);

            this.request = requestParam;

            Action<TContext> wrappedAction = this.DefaultExecuteAction;

            foreach (var prePostAction in this.prePostActions.Reverse())
            {
                var copy = wrappedAction;
                wrappedAction = c =>
                {
                    prePostAction.Execute(c, copy);
                };
            }

            // Todo: handle error of a pipeline
            wrappedAction(context);

            if (context.Exception != null)
            {
                return PipelineResult<TResponse, TContext>.CreateFailure(ErrorResult.FromException(context.Exception));
            }

            var result = GetResultOrDefault<TResponse>(context.Result);
            return PipelineResult<TResponse, TContext>.CreateSuccess(result, context);
        }

        protected virtual TContext InitializeContext(IDictionary<string, object> initialContext)
        {
            var context = new TContext();
            var initialContextValue = initialContext ?? new Dictionary<string, object>();
            foreach (var keyValue in initialContextValue)
            {
                context.Add(keyValue);
            }

            return context;
        }

        private void DefaultExecuteAction(TContext contextParam)
        {
            var result = default(TResponse);

            try
            {
                if (!contextParam.Abort)
                {
                    result = this.commandAction.Execute(this.request, contextParam);
                }
            }
            catch (Exception e)
            {
                contextParam.Exception = e;
                throw;
            }

            contextParam.Result = result;
        }

        private static T GetResultOrDefault<T>(object input)
        {
            T result;

            try
            {
                result = (T) input;
            }
            catch (Exception)
            {
                result = default(T);
            }

            return result;
        }
    }
}