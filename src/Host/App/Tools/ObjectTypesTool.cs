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
/// Provides MCP tool metadata and execution for object type entries. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class ObjectTypesTool : IMcpTool
{
    private readonly IObjectTypes _types;

    /// <summary>
    /// Creates object type tool with provided entries implementation. Usage example: IMcpTool tool = new ObjectTypesTool(types).
    /// </summary>
    /// <param name="types">Object type entries provider.</param>
    public ObjectTypesTool(IObjectTypes types)
    {
        _types = types;
    }

    /// <summary>
    /// Creates object type tool. Usage example: IMcpTool tool = new ObjectTypesTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public ObjectTypesTool(ITerminal terminal, ILogger logger)
        : this(new WsObjectTypes(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "object-types";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"objectTypes":{"type":"array","description":"Object type dictionary entries","items":{"type":"object","properties":{"IdObjectType":{"type":"integer","description":"Object type identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"CodeObjectType":{"type":"string","description":"Object type code"},"NameObjectType":{"type":"string","description":"Object type name"},"ShortNameObjectType":{"type":"string","description":"Object type short name"}},"required":["IdObjectType","IdObjectGroup","CodeObjectType","NameObjectType","ShortNameObjectType"],"additionalProperties":false}}},"required":["objectTypes"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Object types", Description = "Returns object type dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _types.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
