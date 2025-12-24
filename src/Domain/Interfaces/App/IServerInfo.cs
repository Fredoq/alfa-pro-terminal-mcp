using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines server info retrieval. Usage example: Implementation info = item.Info().
/// </summary>
public interface IServerInfo
{
    /// <summary>
    /// Returns server info. Usage example: Implementation info = item.Info().
    /// </summary>
    Implementation Info();
}
