using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for object group entries. Usage example: JsonNode node = new ObjectGroupSchema().Node(item).
/// </summary>
internal sealed class ObjectGroupSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an object group schema with fields. Usage example: var schema = new ObjectGroupSchema().
    /// </summary>
    public ObjectGroupSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdObjectGroup"),
            new TextRule("NameObjectGroup")
        ]);
    }

    /// <summary>
    /// Returns an output node for the object group element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
