using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines response formatting for MCP tool calls. Usage example: CallToolResult result = formatter.Result(entries).
/// </summary>
public interface IContent
{
    /// <summary>
    /// Returns a tool result with structured content. Usage example: CallToolResult result = formatter.Result(entries).
    /// </summary>
    CallToolResult Result(IEntries entries);
}
