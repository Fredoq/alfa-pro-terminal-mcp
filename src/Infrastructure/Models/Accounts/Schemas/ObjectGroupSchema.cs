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
    /// <summary>
    /// Initializes a new instance of <see cref="ObjectGroupSchema"/> configured with rules for object group entries.
    /// </summary>
    /// <remarks>
    /// The constructed schema contains a <c>WholeRule</c> for "IdObjectGroup" and a <c>TextRule</c> for "NameObjectGroup".
    /// </remarks>
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
    /// <summary>
/// Produces a JsonNode representing an object group according to this schema.
/// </summary>
/// <param name="node">Source JsonObject containing fields for the object group.</param>
/// <returns>The JsonNode representing the object group.</returns>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}