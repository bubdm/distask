using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers
{
    public sealed class RecyclingEventArgs : EventArgs
    {
        public RecyclingEventArgs()
        {
            this.Timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp { get; }
    }
}
