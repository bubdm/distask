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

namespace Distask.Tests.Integration.Master.Config
{
    /// <summary>
    /// Represents the configuration object for the integration tests.
    /// </summary>
    public sealed class IntegrationTestHostConfig
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestHostConfig"/> class.
        /// </summary>
        public IntegrationTestHostConfig()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationTestHostConfig"/> class.
        /// </summary>
        /// <param name="numOfThreads">The number of threads.</param>
        public IntegrationTestHostConfig(int numOfThreads)
        {
            this.NumberOfThreads = numOfThreads;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the availability checker.
        /// </summary>
        /// <value>
        /// The availability checker.
        /// </value>
        public ObjectInitializerConfig AvailabilityChecker { get; set; }

        /// <summary>
        /// Gets or sets the number of threads.
        /// </summary>
        /// <value>
        /// The number of threads.
        /// </value>
        public int NumberOfThreads { get; set; }

        /// <summary>
        /// Gets or sets the router.
        /// </summary>
        /// <value>
        /// The router.
        /// </value>
        public ObjectInitializerConfig Router { get; set; }

        #endregion Public Properties
    }
}