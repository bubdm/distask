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
    public class TaskDispatcherConfig
    {
        #region Public Fields

        public static readonly TaskDispatcherConfig AnyAddressDefaultPort = new TaskDispatcherConfig("0.0.0.0", Utils.Constants.MasterDefaultPort);

        #endregion Public Fields

        #region Public Constructors

        public TaskDispatcherConfig(string host, int port)
            : this(host, port, BrokerClientConfig.DefaultBrokerClientConfig)
        {
        }

        public TaskDispatcherConfig(string host, int port, BrokerClientConfig brokerClientConfig)
        {
            Host = host;
            Port = port;
            this.BrokerClient = brokerClientConfig;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Host { get; set; }

        public int Port { get; set; }

        public BrokerClientConfig BrokerClient { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static TaskDispatcherConfig AnyAddress(int port) => new TaskDispatcherConfig("0.0.0.0", port);

        #endregion Public Methods
    }
}