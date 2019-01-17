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

namespace Distask.TaskDispatchers.Config
{
    public class TaskDispatcherConfiguration
    {
        #region Public Fields

        public static readonly TaskDispatcherConfiguration AnyAddressDefaultPort = new TaskDispatcherConfiguration("0.0.0.0", Utils.Constants.TaskDispatcherDefaultPort);

        #endregion Public Fields

        #region Public Constructors

        public TaskDispatcherConfiguration(string host, int port)
            : this(host, port, RecyclingConfiguration.Default, BrokerClientConfiguration.Default)
        {
        }

        public TaskDispatcherConfiguration(RecyclingConfiguration recyclingConfiguration, BrokerClientConfiguration brokerClientConfiguration)
            : this("0.0.0.0", Utils.Constants.TaskDispatcherDefaultPort, recyclingConfiguration, brokerClientConfiguration)
        { }

        public TaskDispatcherConfiguration(string host, 
            int port, 
            RecyclingConfiguration recyclingConfiguration, 
            BrokerClientConfiguration brokerClientConfiguration)
        {
            Host = host;
            Port = port;
            this.RecyclingConfiguration = recyclingConfiguration;
            this.BrokerClientConfiguration = brokerClientConfiguration;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Host { get; set; }

        public int Port { get; set; }

        public RecyclingConfiguration RecyclingConfiguration { get; set; }

        public BrokerClientConfiguration BrokerClientConfiguration { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static TaskDispatcherConfiguration AnyAddress(int port) => new TaskDispatcherConfiguration("0.0.0.0", port);

        #endregion Public Methods
    }
}