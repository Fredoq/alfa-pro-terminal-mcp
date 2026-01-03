namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines application profile values. Usage example: string name = item.ServerName().
/// </summary>
public interface IApplicationProfile
{
    /// <summary>
    /// Returns server name. Usage example: string name = item.ServerName().
    /// </summary>
    string ServerName();

    /// <summary>
    /// Returns application title. Usage example: string title = item.Title().
    /// </summary>
    string Title();

    /// <summary>
    /// Returns MCP version. Usage example: string version = item.Version().
    /// </summary>
    string Version();
}
