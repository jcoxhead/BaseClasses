using System.Collections.Generic;

namespace Schroders.ServiceBase.Commands
{
    public interface ICommand<in TRequest, out TResponse>
    {
        TResponse Execute(TRequest request, IDictionary<string, object> context);
    }
}