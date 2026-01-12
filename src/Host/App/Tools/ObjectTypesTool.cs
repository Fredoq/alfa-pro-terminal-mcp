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
    /// <summary>
    /// Initializes a new instance of ObjectTypesTool using the specified object types provider.
    /// </summary>
    /// <param name="types">Provider used to retrieve object type entries for the tool.</param>
    public ObjectTypesTool(IObjectTypes types)
    {
        _types = types;
    }

    /// <summary>
    /// Creates object type tool. Usage example: IMcpTool tool = new ObjectTypesTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <summary>
    /// Initializes a new ObjectTypesTool that retrieves object type entries using the provided terminal and logger.
    /// </summary>
    /// <param name="terminal">Terminal used to access object type provider services.</param>
    /// <param name="logger">Logger used for diagnostic and operational logging.</param>
    public ObjectTypesTool(ITerminal terminal, ILogger logger)
        : this(new WsObjectTypes(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// <summary>
/// Tool identifier for this MCP tool.
/// </summary>
/// <returns>The tool name "object-types".</returns>
    public string Name() => "object-types";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// <summary>
    /// Builds the MCP tool metadata for "object-types".
    /// </summary>
    /// <remarks>
    /// The returned Tool accepts an empty object as input and produces an object with a required
    /// "objectTypes" property: an array of object entries each containing
    /// IdObjectType, IdObjectGroup, CodeObjectType, NameObjectType, and ShortNameObjectType.
    /// The tool is annotated as read-only and idempotent.
    /// </remarks>
    /// <returns>
    /// A Tool configured for the "object-types" operation with the described input/output schemas and annotations.
    /// </returns>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"objectTypes":{"type":"array","description":"Object type dictionary entries","items":{"type":"object","properties":{"IdObjectType":{"type":"integer","description":"Object type identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"CodeObjectType":{"type":"string","description":"Object type code"},"NameObjectType":{"type":"string","description":"Object type name"},"ShortNameObjectType":{"type":"string","description":"Object type short name"}},"required":["IdObjectType","IdObjectGroup","CodeObjectType","NameObjectType","ShortNameObjectType"],"additionalProperties":false}}},"required":["objectTypes"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Object types", Description = "Returns object type dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// <summary>
    /// Retrieve object type dictionary entries and return them as structured JSON and a textual JSON block.
    /// </summary>
    /// <param name="data">Input values passed to the tool (ignored by this implementation).</param>
    /// <param name="token">Cancellation token to cancel the retrieval operation.</param>
    /// <returns>A CallToolResult whose <c>StructuredContent</c> is a JSON node of object type entries and whose <c>Content</c> is a single text block containing the same JSON as a string.</returns>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _types.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}