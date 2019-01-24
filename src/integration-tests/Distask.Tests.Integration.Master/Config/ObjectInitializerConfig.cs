using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Tests.Integration.Master.Config
{
    public class ObjectInitializerConfig
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public List<ObjectInitializerParameterConfig> Parameters { get; set; }
    }
}
