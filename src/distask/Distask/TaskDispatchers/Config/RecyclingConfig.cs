using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Config
{
    public sealed class RecyclingConfig
    {
        private static readonly TimeSpan DefaultInterval = TimeSpan.FromMinutes(30);

        public RecyclingConfig()
        {
            this.Interval = DefaultInterval;
        }

        public RecyclingConfig(TimeSpan interval)
        {
            this.Interval = interval;
        }

        public RecyclingConfig(string interval)
        {
            if (TimeSpan.TryParse(interval, out var v))
            {
                this.Interval = v;
            }
            else
            {
                this.Interval = DefaultInterval;
            }
        }

        public TimeSpan Interval { get; set; }
    }
}
