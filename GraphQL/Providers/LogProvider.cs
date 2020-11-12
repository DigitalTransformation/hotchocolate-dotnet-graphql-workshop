namespace GraphQL.Providers
{
    using Microsoft.Extensions.Logging;

    public static class LogProvider
    {
        public static ILoggerFactory _loggerFactory { get; set; } = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(_ =>
            {
                // Set Log Level
                builder.SetMinimumLevel(LogLevel.Information);
            });
        });

        public static ILogger CreateLogger<T>() => _loggerFactory.CreateLogger<T>();

        public static ILogger CreateLogger(string categoryName) => _loggerFactory.CreateLogger(categoryName);
    }
}