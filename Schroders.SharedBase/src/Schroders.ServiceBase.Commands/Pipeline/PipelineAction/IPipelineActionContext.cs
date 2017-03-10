using System;
using System.Collections.Generic;

namespace Schroders.ServiceBase.Commands.Pipeline.PipelineAction
{
    public interface IPipelineActionContext : IDictionary<string, object>
    {
        bool Abort { get; set; }

        Exception Exception { get; set; }

        object Result { get; set; }
    }
}