using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;

/// <summary>
/// Defines response formatting for MCP tool calls. Usage example: CallToolResult result = formatter.Result(entries, "accounts").
/// </summary>
internal interface IContent
{
    /// <summary>
    /// Returns a tool result with structured content wrapped in the provided root property. Usage example: CallToolResult result = formatter.Result(entries, "accounts").
    /// </summary>
    /// <param name="entries">Structured entries payload.</param>
    /// <param name="name">Root property name.</param>
    CallToolResult Result(IEntries entries, string name);
}
