using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool execution using a tool plan and entries source. Usage example: IMcpTool tool = new McpTool(entries, plan).
/// </summary>
internal sealed class McpTool : IMcpTool
{
    private readonly IEntriesSource _entries;
    private readonly IToolPlan _plan;

    /// <summary>
    /// Creates MCP tool using provided entries and plan. Usage example: IMcpTool tool = new McpTool(entries, plan).
    /// </summary>
    /// <param name="entries">Entries provider.</param>
    /// <param name="plan">Tool plan.</param>
    public McpTool(IEntriesSource entries, IToolPlan plan)
    {
        _entries = entries;
        _plan = plan;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => _plan.Tool().Name;

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool() => _plan.Tool();

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _entries.Entries(_plan.Payload(data), token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
