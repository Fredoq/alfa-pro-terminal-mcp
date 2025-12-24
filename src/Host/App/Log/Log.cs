using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides logger from factory. Usage example: ILog log = new Log(factory).
/// </summary>
internal sealed class Log : ILog
{
    private readonly ILoggerFactory _factory;

    /// <summary>
    /// Creates logger wrapper. Usage example: ILog log = new Log(factory).
    /// </summary>
    /// <param name="factory">Logger factory provider.</param>
    public Log(ILoggerFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Disposes the logger factory. Usage example: item.Dispose().
    /// </summary>
    public void Dispose() => _factory.Dispose();

    /// <summary>
    /// Returns logger. Usage example: ILogger log = item.Logger().
    /// </summary>
    public ILogger Logger() => _factory.CreateLogger("terminal");

    /// <summary>
    /// Returns factory. Usage example: ILoggerFactory factory = item.Factory().
    /// </summary>
    public ILoggerFactory Factory() => _factory;
}
