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
using System.Threading.Tasks;

namespace Distask.TaskDispatchers.AvailabilityCheckers
{
    /// <summary>
    /// Represents that the implemented classes are the checkers which determines whether the
    /// specified broker client is available.
    /// </summary>
    public interface IAvailabilityChecker
    {
        #region Public Methods

        /// <summary>
        /// Determines whether the specified broker client is available.
        /// </summary>
        /// <param name="client">The client to be checked.</param>
        /// <returns>The <see cref="Task"/> which executes the checking and returns a <see cref="bool"/>
        /// value that indicates whether the specified client is available.</returns>
        Task<bool> IsAvailableAsync(IBrokerClient client);

        #endregion Public Methods
    }
}