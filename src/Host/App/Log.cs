using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines logger retrieval. Usage example: ILogger log = item.Logger().
/// </summary>
internal interface ILog
{
    /// <summary>
    /// Returns logger. Usage example: ILogger log = item.Logger().
    /// </summary>
    ILogger Logger();
}

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
    /// Returns logger. Usage example: ILogger log = item.Logger().
    /// </summary>
    public ILogger Logger() => _factory.CreateLogger("terminal");
}
