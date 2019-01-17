using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers.Config
{
    public sealed class RecyclingConfiguration
    {
        private static readonly TimeSpan DefaultInterval = TimeSpan.FromMinutes(30);

        public static readonly RecyclingConfiguration Default = new RecyclingConfiguration();

        public RecyclingConfiguration()
        {
            this.Interval = DefaultInterval;
        }

        public RecyclingConfiguration(TimeSpan interval)
        {
            this.Interval = interval;
        }

        public RecyclingConfiguration(string interval)
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
