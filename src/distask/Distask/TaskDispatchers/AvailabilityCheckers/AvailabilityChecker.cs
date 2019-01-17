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
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    /// <summary>
    /// Represents the base class for availability checkers.
    /// </summary>
    /// <seealso cref="Distask.TaskDispatchers.AvailabilityCheckers.IAvailabilityChecker" />
    public abstract class AvailabilityChecker : IAvailabilityChecker
    {

        #region Protected Fields

        protected readonly ILogger logger;

        #endregion Protected Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailabilityChecker"/> class.
        /// </summary>
        /// <param name="logger">The logger which emits the logging information.</param>
        protected AvailabilityChecker(ILogger logger)
            => this.logger = logger;

        #endregion Protected Constructors

        #region Public Methods

        /// <summary>
        /// Determines whether the specified broker client is available.
        /// </summary>
        /// <param name="client">The client to be checked.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" /> which executes the checking and returns a <see cref="T:System.Boolean" />
        /// value that indicates whether the specified client is available.
        /// </returns>
        public async Task<bool> IsAvailableAsync(IBrokerClient client)
        {
            if (client.State.LifetimeState != BrokerClientLifetimeState.Alive)
            {
                return false;
            }

            return await IsAvailableInternalAsync(client);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract Task<bool> IsAvailableInternalAsync(IBrokerClient client);

        #endregion Protected Methods
    }
}