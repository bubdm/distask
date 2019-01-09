using Distask.TaskDispatchers.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    public interface IAvailabilityChecker
    {
        Task<bool> IsAvailableAsync(IBrokerClient client);
    }
}
