﻿using Distask.TaskDispatchers.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers
{
    public interface ITaskDispatcher : IDisposable
    {
        event EventHandler<BrokerClientRegisteredEventArgs> BrokerClientRegistered;

        Task<ResponseMessage> DispatchAsync(RequestMessage requestMessage, string group = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
