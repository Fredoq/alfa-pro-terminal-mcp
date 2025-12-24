using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines server info retrieval. Usage example: Implementation info = item.Info().
/// </summary>
internal interface IServerInfo
{
    /// <summary>
    /// Returns server info. Usage example: Implementation info = item.Info().
    /// </summary>
    Implementation Info();
}
