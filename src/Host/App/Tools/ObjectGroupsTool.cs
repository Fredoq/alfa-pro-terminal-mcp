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
    /// <summary>
    /// Initializes a new instance of ObjectGroupsTool with the specified object group entries provider.
    /// </summary>
    /// <param name="groups">The provider used to obtain object group entries.</param>
    public ObjectGroupsTool(IObjectGroups groups)
    {
        _groups = groups;
    }

    /// <summary>
    /// Creates object group tool. Usage example: IMcpTool tool = new ObjectGroupsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <summary>
    /// Initializes a new ObjectGroupsTool using the provided terminal and logger to create the object-groups provider.
    /// </summary>
    /// <param name="terminal">Terminal used to construct the object-groups provider.</param>
    /// <param name="logger">Logger used by the object-groups provider.</param>
    public ObjectGroupsTool(ITerminal terminal, ILogger logger)
        : this(new WsObjectGroups(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// <summary>
/// Gets the unique name used to identify this MCP tool.
/// </summary>
/// <returns>The tool name "object-groups".</returns>
    public string Name() => "object-groups";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// <summary>
    /// Creates metadata for the "object-groups" tool, including its input and output JSON schemas and execution annotations.
    /// </summary>
    /// <returns>A <see cref="Tool"/> describing the tool's name, title, description, input/output schemas and annotations.</returns>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"objectGroups":{"type":"array","description":"Object group dictionary entries","items":{"type":"object","properties":{"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"NameObjectGroup":{"type":"string","description":"Object group name"}},"required":["IdObjectGroup","NameObjectGroup"],"additionalProperties":false}}},"required":["objectGroups"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Object groups", Description = "Returns object group dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// <summary>
    /// Fetches object group entries and returns them as both structured JSON and a text content block.
    /// </summary>
    /// <param name="data">Input dictionary (not used by this tool).</param>
    /// <param name="token">Cancellation token to cancel the fetch operation.</param>
    /// <returns>A CallToolResult whose <c>StructuredContent</c> is a JsonNode of the object group entries and whose <c>Content</c> contains a single TextContentBlock with the node serialized to JSON.</returns>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _groups.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}