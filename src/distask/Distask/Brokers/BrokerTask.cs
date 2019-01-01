using Distask.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.Brokers
{
    public abstract class BrokerTask
    {
        public abstract string Name { get; }

        public abstract Task<DistaskResponse> ExecuteAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default(CancellationToken));

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is BrokerTask task &&
                string.Equals(task.Name, this.Name);
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Name) ? base.GetHashCode() : Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
