using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Client
{
    public sealed class BrokerClientCircuitBrokenEventArgs : EventArgs
    {
        public BrokerClientCircuitBrokenEventArgs(Exception exception, TimeSpan timeSpan)
        {
            this.Exception = exception;
            this.TimeSpan = timeSpan;
        }

        public TimeSpan TimeSpan { get; }

        public Exception Exception { get; }
    }
}
