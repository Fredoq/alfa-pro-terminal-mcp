namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using Microsoft.Extensions.Logging;

/// <summary>
/// Minimal logger implementation for tests. Usage example: new LoggerFake().
/// </summary>
internal sealed class LoggerFake : ILogger
{
    /// <summary>
    /// Begins a no-op scope. Usage example: logger.BeginScope(state).
    /// </summary>
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => new ScopeFake();

    /// <summary>
    /// Reports that all log levels are enabled. Usage example: logger.IsEnabled(LogLevel.Debug).
    /// </summary>
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <summary>
    /// Ignores log records. Usage example: logger.Log(level, id, state, ex, formatter).
    /// </summary>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }
}
