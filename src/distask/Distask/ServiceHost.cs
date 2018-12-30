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

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distask
{
    /// <summary>
    /// Represents the generic service host.
    /// </summary>
    /// <seealso cref="Distask.IServiceHost" />
    public abstract class ServiceHost : IServiceHost
    {
        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        protected ServiceHost()
        {
        }

        #endregion Protected Constructors

        #region Private Destructors

        /// <summary>
        /// Finalizes an instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        ~ServiceHost()
        {
            Dispose(false);
        }

        #endregion Private Destructors

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public abstract Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public abstract Task StopAsync(CancellationToken cancellationToken);

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing) { }

        #endregion Protected Methods
    }
}