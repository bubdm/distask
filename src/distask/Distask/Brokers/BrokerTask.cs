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

using Distask.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distask.Brokers
{
    /// <summary>
    /// Represents a task that can be executed by a broker.
    /// </summary>
    public abstract class BrokerTask
    {
        #region Protected Fields

        protected readonly ILogger logger;

        #endregion Protected Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerTask"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected BrokerTask(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Gets the name of the task.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is BrokerTask task &&
                string.Equals(task.Name, this.Name);
        }

        public async Task<DistaskResponse> ExecuteAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.ExecuteInternalAsync(parameters, cancellationToken);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => string.IsNullOrEmpty(Name) ? base.GetHashCode() : Name.GetHashCode();

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract Task<DistaskResponse> ExecuteInternalAsync(IEnumerable<string> parameters, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Protected Methods
    }
}