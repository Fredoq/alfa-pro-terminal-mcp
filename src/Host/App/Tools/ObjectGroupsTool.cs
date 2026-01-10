using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for object group entries. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class ObjectGroupsTool : IMcpTool
{
    private readonly IObjectGroups _groups;

    /// <summary>
    /// Creates object group tool with provided entries implementation. Usage example: IMcpTool tool = new ObjectGroupsTool(groups).
    /// </summary>
    /// <param name="groups">Object group entries provider.</param>
    public ObjectGroupsTool(IObjectGroups groups)
    {
        _groups = groups;
    }

    /// <summary>
    /// Creates object group tool. Usage example: IMcpTool tool = new ObjectGroupsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public ObjectGroupsTool(ITerminal terminal, ILogger logger)
        : this(new WsObjectGroups(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "object-groups";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"objectGroups":{"type":"array","description":"Object group dictionary entries","items":{"type":"object","properties":{"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"NameObjectGroup":{"type":"string","description":"Object group name"}},"required":["IdObjectGroup","NameObjectGroup"],"additionalProperties":false}}},"required":["objectGroups"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Object groups", Description = "Returns object group dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _groups.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
