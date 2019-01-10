using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Distask.TaskDispatchers.Client;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public sealed class OrAvailabilityChecker : BinaryAvailabilityChecker
    {
        public OrAvailabilityChecker(IAvailabilityChecker leftChecker, IAvailabilityChecker rightChecker) 
            : base(leftChecker, rightChecker)
        { }

        public override async Task<bool> IsAvailableAsync(IBrokerClient client) 
            => await this.leftChecker.IsAvailableAsync(client) || await this.rightChecker.IsAvailableAsync(client);
    }
}
