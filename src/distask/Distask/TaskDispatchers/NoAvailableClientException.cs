using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers
{
    public sealed class NoAvailableClientException : TaskDispatchException
    {
        public NoAvailableClientException(string forGroup)
            : base ($"No broker client available in group '{forGroup}' that can accept the distributed tasks.")
        {

        }
    }
}
