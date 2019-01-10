using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Distask.TaskDispatchers.Client;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public abstract class BinaryAvailabilityChecker : IAvailabilityChecker
    {
        protected readonly IAvailabilityChecker leftChecker;
        protected readonly IAvailabilityChecker rightChecker;

        protected BinaryAvailabilityChecker(IAvailabilityChecker leftChecker, IAvailabilityChecker rightChecker)
        {
            this.leftChecker = leftChecker ?? throw new ArgumentNullException(nameof(leftChecker));
            this.rightChecker = rightChecker ?? throw new ArgumentNullException(nameof(rightChecker));
        }

        public abstract Task<bool> IsAvailableAsync(IBrokerClient client);
    }
}
