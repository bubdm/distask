using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Config
{
    public class ResiliencyConfiguration
    {
        public static readonly ResiliencyConfiguration Default = new ResiliencyConfiguration
        {
            CircuitBreakOnExceptions = 3,
            CircuitBreakMilliseconds = 5000,
            Enabled = true
        };

        public int CircuitBreakOnExceptions { get; set; }

        public int CircuitBreakMilliseconds { get; set; }

        public bool Enabled { get; set; }
    }
}
