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
    /// <summary>
    /// Initializes a new ObjectTypeSchema configured with rules for IdObjectType, IdObjectGroup, CodeObjectType, NameObjectType, and ShortNameObjectType.
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
    /// <summary>
/// Produce a JsonNode representation of the provided JSON object using the object-type schema.
/// </summary>
/// <param name="node">Source JSON object to convert using this schema.</param>
/// <returns>The JsonNode constructed from the provided JsonObject.</returns>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}