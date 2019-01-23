﻿using Distask.TaskDispatchers.Client;
using Distask.TaskDispatchers.Config;
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

        event EventHandler<BrokerClientRecycledEventArgs> BrokerClientRecycled;

        event EventHandler<BrokerClientDisposedEventArgs> BrokerClientDisposed;

        event EventHandler<RecyclingEventArgs> RecyclingStarted;

        event EventHandler<RecyclingEventArgs> RecyclingCompleted;

        TaskDispatcherConfiguration Configuration { get; }

        Task<ResponseMessage> DispatchAsync(RequestMessage requestMessage, string group = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
