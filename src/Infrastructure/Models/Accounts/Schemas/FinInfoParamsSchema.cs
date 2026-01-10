using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for fin info params entries. Usage example: JsonNode node = new FinInfoParamsSchema().Node(item).
/// </summary>
internal sealed class FinInfoParamsSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a fin info params schema with fields. Usage example: var schema = new FinInfoParamsSchema().
    /// </summary>
    public FinInfoParamsSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdFi"),
            new WholeRule("IdSession"),
            new WholeRule("IdTradePeriodStatus"),
            new TextRule("SessionDate"),
            new WholeRule("Lot"),
            new RealRule("PriceStep"),
            new RealRule("PriceStepCost"),
            new WholeRule("IdObjectCurrency"),
            new RealRule("PSTNKD"),
            new RealRule("UpPrice"),
            new RealRule("DownPrice"),
            new RealRule("GtBuy"),
            new RealRule("GtSell"),
            new RealRule("FaceValue")
        ]);
    }

    /// <summary>
    /// Returns an output node for the fin info params element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
