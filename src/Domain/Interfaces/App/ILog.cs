using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines logger retrieval. Usage example: ILogger log = item.Logger().
/// </summary>
public interface ILog : IDisposable
{
    /// <summary>
    /// Returns logger. Usage example: ILogger log = item.Logger().
    /// </summary>
    ILogger Logger();

    /// <summary>
    /// Returns factory. Usage example: ILoggerFactory factory = item.Factory().
    /// </summary>
    ILoggerFactory Factory();
}
