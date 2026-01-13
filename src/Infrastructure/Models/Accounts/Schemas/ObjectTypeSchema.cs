using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for object type entries. Usage example: JsonNode node = new ObjectTypeSchema().Node(item).
/// </summary>
internal sealed class ObjectTypeSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an object type schema with fields. Usage example: var schema = new ObjectTypeSchema().
    /// </summary>
    public ObjectTypeSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdObjectType"),
            new WholeRule("IdObjectGroup"),
            new TextRule("CodeObjectType"),
            new TextRule("NameObjectType"),
            new TextRule("ShortNameObjectType")
        ]);
    }

    /// <summary>
    /// Returns an output node for the object type element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
