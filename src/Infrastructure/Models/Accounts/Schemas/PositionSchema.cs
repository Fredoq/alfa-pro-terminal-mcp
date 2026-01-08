using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for a single position entry. Usage example: JsonNode node = new PositionSchema().Node(element).
/// </summary>
internal sealed class PositionSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a position schema with fields. Usage example: var schema = new PositionSchema().
    /// </summary>
    public PositionSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdPosition"),
            new WholeRule("IdAccount"),
            new WholeRule("IdSubAccount"),
            new WholeRule("IdRazdel"),
            new WholeRule("IdObject"),
            new WholeRule("IdFiBalance"),
            new WholeRule("IdBalanceGroup"),
            new RealRule("AssetsPercent"),
            new RealRule("PSTNKD"),
            new FlagRule("IsMoney"),
            new FlagRule("IsRur"),
            new RealRule("UchPrice"),
            new RealRule("TorgPos"),
            new RealRule("Price"),
            new RealRule("DailyPL"),
            new RealRule("DailyPLPercentToMarketCurPrice"),
            new RealRule("BackPos"),
            new RealRule("PrevQuote"),
            new RealRule("TrnIn"),
            new RealRule("TrnOut"),
            new RealRule("DailyBuyVolume"),
            new RealRule("DailySellVolume"),
            new RealRule("DailyBuyQuantity"),
            new RealRule("DailySellQuantity"),
            new RealRule("NKD"),
            new RealRule("PriceStep"),
            new WholeRule("Lot"),
            new RealRule("NPLtoMarketCurPrice"),
            new RealRule("NPLPercent"),
            new RealRule("PlanLong"),
            new RealRule("PlanShort")
        ]);
    }

    /// <summary>
    /// Returns an output node for the position element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
