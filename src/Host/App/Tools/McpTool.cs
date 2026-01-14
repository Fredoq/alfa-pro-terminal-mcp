using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool execution using tool metadata and payload plan. Usage example: IMcpTool tool = new McpTool(entries, tool, payload).
/// </summary>
internal sealed class McpTool : IMcpTool
{
    private readonly IEntriesSource _entries;
    private readonly Tool _tool;
    private readonly IPayloadPlan _payload;

    /// <summary>
    /// Creates MCP tool using provided entries, tool metadata, and payload plan. Usage example: IMcpTool tool = new McpTool(entries, tool, payload).
    /// </summary>
    /// <param name="entries">Entries provider.</param>
    /// <param name="tool">Tool metadata.</param>
    /// <param name="payload">Payload plan.</param>
    public McpTool(IEntriesSource entries, Tool tool, IPayloadPlan payload)
    {
        _entries = entries;
        _tool = tool;
        _payload = payload;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => _tool.Name;

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool() => _tool;

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _entries.Entries(_payload.Payload(data), token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
