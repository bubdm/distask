using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Tests.Integration.Master.Config
{
    public sealed class IntegrationTestHostConfig
    {
        public IntegrationTestHostConfig()
        {
        }

        public IntegrationTestHostConfig(int numOfThreads)
        {
            this.NumberOfThreads = numOfThreads;
        }

        public int NumberOfThreads { get; set; }
    }
}
