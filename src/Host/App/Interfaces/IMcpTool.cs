using System.Text.Json;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;

/// <summary>
/// Defines MCP tool metadata and execution contract. Usage example: Tool tool = item.Tool().
/// </summary>
internal interface IMcpTool
{
    /// <summary>
    /// Returns the tool name. Usage example: string name = item.Name().
    /// </summary>
    string Name();

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = item.Tool().
    /// </summary>
    Tool Tool();

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await item.Result(args, token).
    /// </summary>
    ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token);
}
