namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines MCP version retrieval. Usage example: string version = item.Version().
/// </summary>
public interface IMcpVersion
{
    /// <summary>
    /// Returns MCP version. Usage example: string version = item.Version().
    /// </summary>
    string Version();
}
