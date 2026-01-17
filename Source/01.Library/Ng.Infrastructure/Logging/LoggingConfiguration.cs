namespace Ng.Infrastructure.Logging;

public class LoggingConfiguration
{
    public const string Section = "LoggingConfiguration";
    public string LogLevel { get; set; } = "Info";
    public bool EnableFileLogging { get; set; } = true;
    public bool EnableConsoleLogging { get; set; } = true;
    public bool EnableStructuredLogging { get; set; } = true;
    public int MaxArchiveFiles { get; set; } = 30;
}
