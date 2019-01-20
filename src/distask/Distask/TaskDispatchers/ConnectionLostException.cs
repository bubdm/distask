using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers
{
    public sealed class ConnectionLostException : TaskDispatchException
    {
        public ConnectionLostException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
