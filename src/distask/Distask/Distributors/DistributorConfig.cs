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

namespace Distask.Distributors
{
    public class DistributorConfig
    {
        #region Public Fields

        public static readonly DistributorConfig AnyAddressDefaultPort = new DistributorConfig("0.0.0.0", Utils.Constants.MasterDefaultPort);

        #endregion Public Fields

        #region Public Constructors

        public DistributorConfig(string host, int port)
            : this(host, port, Utils.Constants.DefaultRetryCount)
        {
        }

        public DistributorConfig(string host, int port, int retryCount)
        {
            Host = host;
            Port = port;
            RetryCount = retryCount;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Host { get; set; }

        public int Port { get; set; }

        public int RetryCount { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static DistributorConfig AnyAddress(int port) => new DistributorConfig("0.0.0.0", port);

        #endregion Public Methods
    }
}