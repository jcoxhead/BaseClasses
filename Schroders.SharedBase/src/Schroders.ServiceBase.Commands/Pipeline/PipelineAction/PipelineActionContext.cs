using System;
using System.Collections.Generic;

namespace Schroders.ServiceBase.Commands.Pipeline.PipelineAction
{
    public class PipelineActionContext : Dictionary<string, object>, IPipelineActionContext
    {
        public bool Abort { get; set; }

        public Exception Exception { get; set; }

        public object Result { get; set; }
    }
}