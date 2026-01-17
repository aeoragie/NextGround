using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NLog;
using NLog.Config;
using NLog.Extensions.Hosting;
using NLog.Targets;

namespace Ng.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IHostBuilder ConfigurationNextGroundLogger(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.ConfigureLogging((context, logging) =>
        {
            logging.ClearProviders();
            var path = GetLogConfigPath();
            ConfigureLogger(path, configuration);
        }).UseNLog();
        return hostBuilder;
    }

    public static IServiceCollection AddNextGroundLogger(this IServiceCollection services)
    {
        services.AddSingleton<NLog.ILogger>(LogManager.GetCurrentClassLogger());
        return services;
    }

    private static string GetLogConfigPath()
    {
        var baseDirectory = AppContext.BaseDirectory;
        var possiblePaths = Path.Combine(baseDirectory, "Configuration", "Logger.config");

        //GlobalInfrastructure.LoggingDirectory = possiblePaths;

        return possiblePaths;
    }

    private static void ConfigureLogger(string configPath, IConfiguration configuration)
    {
        try
        {
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("Logger configuration file not found.", configPath);
            }

            LogManager.Configuration = new XmlLoggingConfiguration(configPath);

            ApplyAppSettingsToLogger(configuration);
            LogManager.ReconfigExistingLoggers();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to configure logger.", ex);
        }
    }

    private static void ApplyAppSettingsToLogger(IConfiguration configuration)
    {
        var options = configuration.GetSection(LoggingConfiguration.Section).Get<LoggingConfiguration>();
        if (options == null)
        {
            return;
        }

        var config = LogManager.Configuration;
        if (config == null)
        {
            return;
        }

        if (options.EnableFileLogging)
        {
            var fileTarget = config.FindTargetByName<FileTarget>("FileLogger");
            if (fileTarget != null)
            {
                if (options.MaxArchiveFiles > 0)
                {
                    fileTarget.ArchiveOldFileOnStartup = true;
                    fileTarget.MaxArchiveFiles = options.MaxArchiveFiles;
                }
            }
        }

        if (!options.EnableConsoleLogging)
        {
            var consoleTarget = config.FindTargetByName("ConsoleLogger");
            if (consoleTarget != null)
            {
                var rulesToRemove = config.LoggingRules.Where(rule => rule.Targets.Contains(consoleTarget)).ToList();
                foreach (var rule in rulesToRemove)
                {
                    rule.Targets.Remove(consoleTarget);
                }
            }
        }

        if (!string.IsNullOrEmpty(options.LogLevel))
        {
            var logLevel = NLog.LogLevel.FromString(options.LogLevel);
            foreach (var rule in config.LoggingRules)
            {
                if (rule.LoggerNamePattern == "*")
                {
                    rule.SetLoggingLevels(logLevel, NLog.LogLevel.Fatal);
                }
            }
        }
    }
}
