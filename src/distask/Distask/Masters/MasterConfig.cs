using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Masters
{
    public class MasterConfig
    {

        public static readonly MasterConfig AnyAddressDefaultPort = new MasterConfig("0.0.0.0", Utils.Constants.MasterDefaultPort);

        public MasterConfig(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; }

        public int Port { get; set; }

        public static MasterConfig AnyAddress(int port) => new MasterConfig("0.0.0.0", port);

    }
}
