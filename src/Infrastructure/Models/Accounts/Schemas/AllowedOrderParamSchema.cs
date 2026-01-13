using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for allowed order parameter entries. Usage example: JsonNode node = new AllowedOrderParamSchema().Node(item).
/// </summary>
internal sealed class AllowedOrderParamSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an allowed order parameter schema with fields. Usage example: var schema = new AllowedOrderParamSchema().
    /// </summary>
    public AllowedOrderParamSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdAllowedOrderParams"),
            new WholeRule("IdObjectGroup"),
            new WholeRule("IdMarketBoard"),
            new WholeRule("IdOrderType"),
            new WholeRule("IdDocumentType"),
            new WholeRule("IdQuantityType"),
            new WholeRule("IdPriceType"),
            new WholeRule("IdLifeTime"),
            new WholeRule("IdExecutionType")
        ]);
    }

    /// <summary>
    /// Returns an output node for the allowed order parameter element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
