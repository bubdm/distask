using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Client
{
    internal sealed class ExceptionLogEntry
    {
        public ExceptionLogEntry(Exception exception, DateTime occurredOn)
        {
            this.OccurredOn = occurredOn;
            this.Exception = exception;
        }

        public ExceptionLogEntry(Exception exception)
            : this(exception, DateTime.UtcNow)
        {

        }

        public DateTime OccurredOn { get; }

        public Exception Exception { get; }
    }
}
