using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for a limit response entry. Usage example: JsonNode node = new LimitSchema().Node(item).
/// </summary>
internal sealed class LimitSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a limit schema with fields. Usage example: var schema = new LimitSchema().
    /// </summary>
    public LimitSchema()
    {
        _schema = new RulesSchema([new WholeRule("Quantity"), new WholeRule("QuantityForOwnAssets")]);
    }

    /// <summary>
    /// Returns an output node for the limit element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
