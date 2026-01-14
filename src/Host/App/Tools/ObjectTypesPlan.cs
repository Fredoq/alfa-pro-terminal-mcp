using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for object type entries. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class ObjectTypesPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"objectTypes":{"type":"array","description":"Object type dictionary entries","items":{"type":"object","properties":{"IdObjectType":{"type":"integer","description":"Object type identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"CodeObjectType":{"type":"string","description":"Object type code"},"NameObjectType":{"type":"string","description":"Object type name"},"ShortNameObjectType":{"type":"string","description":"Object type short name"}},"required":["IdObjectType","IdObjectGroup","CodeObjectType","NameObjectType","ShortNameObjectType"],"additionalProperties":false}}},"required":["objectTypes"],"additionalProperties":false}""");
        return new Tool { Name = "object-types", Title = "Object types", Description = "Returns object type dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        schema.Ensure(data);
        return new ObjectTypeEntity();
    }
}
