using DJS.Core.Scheduler.Configurations.Remote;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DJS.Core.Scheduler.Configurations
{
    class SchedulerConfigurationProvider : FileConfigurationProvider
    {
        public SchedulerConfigurationProvider(SchedulerConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new JsonConfigurationParser();
            this.Data = parser.Parse(stream, null);
        }
    }
}