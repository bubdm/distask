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

namespace Distask
{
    /// <summary>
    /// Represents the utility class that provides the utility methods for Distask.
    /// </summary>
    public static class Utils
    {
        #region Internal Classes

        /// <summary>
        /// Represents the constant values used across the Distask framework.
        /// </summary>
        public static class Constants
        {
            #region Public Fields

            /// <summary>
            /// Represents the configuration section name for broker hosts.
            /// </summary>
            public const string BrokerHostConfigurationSectionName = "distask.broker";

            /// <summary>
            /// Represents the name of the default broker client group.
            /// </summary>
            public const string DefaultGroupName = "default";

            /// <summary>
            /// Represents the default value of the retry count that the Task Dispatcher will use to re-route
            /// the client request to another broker client.
            /// </summary>
            public const int DefaultRerouteRetryCount = 5;

            /// <summary>
            /// Represents the configuration section name for the task dispatcher.
            /// </summary>
            public const string TaskDispatcherConfigurationSectionName = "distask.dispatcher";

            /// <summary>
            /// Represents the default TCP port to which the task dispatcher listens.
            /// </summary>
            public const int TaskDispatcherDefaultPort = 5919;

            #endregion Public Fields
        }

        #endregion Internal Classes
    }
}