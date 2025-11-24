namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

using System.Diagnostics.CodeAnalysis;
using ModelContextProtocol.Server;

/// <summary>
/// Provides MCP tools for PRO-Terminal interactions. Usage example: var tools = new ProTerminalTools(); string echoed = tools.Echo("ping");
/// </summary>
[SuppressMessage("Performance", "CA1812", Justification = "Activated via MCP tool discovery")]
[McpServerToolType]
internal sealed class ProTerminalTools : IProTerminalTools
{
    /// <summary>
    /// Returns the provided text unchanged. Usage example: string echoed = tools.Echo("hello");
    /// </summary>
    [McpServerTool]
    public string Echo(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        return text;
    }
}

/// <summary>
/// Describes MCP tool contracts exposed by the host. Usage example: IProTerminalTools tools = new ProTerminalTools();.
/// </summary>
internal interface IProTerminalTools
{
    /// <summary>
    /// Returns the provided text unchanged. Usage example: string echoed = tools.Echo("hello");
    /// </summary>
    string Echo(string text);
}
