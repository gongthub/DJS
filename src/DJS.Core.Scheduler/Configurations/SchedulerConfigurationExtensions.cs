using DJS.Core.Scheduler.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DJS.Core.Scheduler.Configurations
{
    public static class SchedulerConfigurationExtensionsstatic
    {
        public static IConfigurationBuilder AddSchedulerFile(this IConfigurationBuilder builder, string path)
        {
            return AddSchedulerFile(builder, provider: null, path: path, optional: false, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddSchedulerFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return AddSchedulerFile(builder, provider: null, path: path, optional: optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddSchedulerFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return AddSchedulerFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
        }

        public static IConfigurationBuilder AddSchedulerFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
        {
            Check.NotNull(builder, "builder");
            Check.CheckCondition(() => string.IsNullOrEmpty(path), "path");
            if (provider == null && Path.IsPathRooted(path))
            {
                provider = new PhysicalFileProvider(Path.GetDirectoryName(path));
                path = Path.GetFileName(path);
            }
            var source = new SchedulerConfigurationSource
            {
                FileProvider = provider,
                Path = path,
                Optional = optional,
                ReloadOnChange = reloadOnChange
            };
            builder.Add(source);
            AppConfig.Configuration = builder.Build();
            return builder;
        }
    }
}