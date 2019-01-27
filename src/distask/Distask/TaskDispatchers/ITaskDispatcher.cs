/****************************************************************************
 *           ___      __             __
 *      ____/ (_)____/ /_____ ______/ /__
 *     / __  / / ___/ __/ __ `/ ___/ //_/
 *    / /_/ / (__  ) /_/ /_/ (__  ) ,<
 *    \__,_/_/____/\__/\__,_/____/_/|_|
 *
 * Copyright (C) 2018-2019 by daxnet, https://github.com/daxnet/distask
 * All rights reserved.
 * Licensed under MIT License.
 * https://github.com/daxnet/distask/blob/master/LICENSE
 ****************************************************************************/

using Distask.TaskDispatchers.Client;
using Distask.TaskDispatchers.Config;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers
{
    public interface ITaskDispatcher : IDisposable
    {
        #region Public Events

        event EventHandler<BrokerClientDisposedEventArgs> BrokerClientDisposed;

        event EventHandler<BrokerClientRecycledEventArgs> BrokerClientRecycled;

        event EventHandler<BrokerClientRegisteredEventArgs> BrokerClientRegistered;

        event EventHandler<RecyclingEventArgs> RecyclingCompleted;

        event EventHandler<RecyclingEventArgs> RecyclingStarted;

        #endregion Public Events

        #region Public Properties

        TaskDispatcherConfiguration Configuration { get; }

        #endregion Public Properties

        #region Public Methods

        Task<ResponseMessage> DispatchAsync(string taskName, IEnumerable<string> parameters, string group = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<ResponseMessage> DispatchAsync(string taskName, string group = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<ResponseMessage> DispatchAsync(RequestMessage requestMessage, string group = null, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Public Methods
    }
}