
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Schroders.ServiceBase.Commands.Pipeline
{
    public class ErrorResult : ReadOnlyCollection<Error>
    {
        public ErrorResult(IList<Error> list)
            : base(list)
        {
        }

        public static ErrorResult FromException(Exception exception)
        {
            return new ErrorResult(new[]
                                       {
                                           new Error("Exception", exception.Message)
                                       });
        }
    }
}