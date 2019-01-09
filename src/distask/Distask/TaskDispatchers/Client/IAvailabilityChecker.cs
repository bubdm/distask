using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.Client
{
    public interface IAvailabilityChecker
    {
        Task<bool> IsAvailableAsync(IBrokerClient client);
    }
}
