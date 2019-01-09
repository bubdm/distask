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

namespace Distask.TaskDispatchers.Client
{
    /// <summary>
    /// Represents the event data contained by the event which occurs when a new broker
    /// has registered to the distributor.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class BrokerClientRegisteredEventArgs : EventArgs
    {

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerClientRegisteredEventArgs"/> class.
        /// </summary>
        /// <param name="group">The group to which the broker belongs.</param>
        /// <param name="name">The name of the broker.</param>
        /// <param name="host">The host name of the machine on which the broker is running.</param>
        /// <param name="port">The port to which the broker listens and waits for the task execution requests.</param>
        internal BrokerClientRegisteredEventArgs(string group, string name, string host, int port)
        {
            this.Group = group;
            this.Name = name;
            this.Host = host;
            this.Port = port;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the group to which the broker belongs.
        /// </summary>
        /// <value>
        /// The group to which the broker belongs.
        /// </value>
        public string Group { get; }

        /// <summary>
        /// Gets the host name of the machine on which the broker is running.
        /// </summary>
        /// <value>
        /// The host name of the machine on which the broker is running.
        /// </value>
        public string Host { get; }

        /// <summary>
        /// Gets the name of the broker.
        /// </summary>
        /// <value>
        /// The name of the broker.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the port to which the broker listens and waits for the task execution requests.
        /// </summary>
        /// <value>
        /// The port to which the broker listens and waits for the task execution requests.
        /// </value>
        public int Port { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Group}:{Name} - {Host}:{Port}";
        }

        #endregion Public Methods
    }
}