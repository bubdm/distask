using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Config
{
    public class ResiliencyConfig
    {
        public static readonly ResiliencyConfig DefaultResiliencyConfig = new ResiliencyConfig
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
