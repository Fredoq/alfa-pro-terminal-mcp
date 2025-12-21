using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;
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
    /// Creates a position schema with described fields. Usage example: var schema = new PositionSchema().
    /// </summary>
    public PositionSchema()
    {
        PositionDescriptions text = new();
        _schema = new RulesSchema([
            new WholeRule("IdPosition", text.Text("IdPosition")),
            new WholeRule("IdAccount", text.Text("IdAccount")),
            new WholeRule("IdSubAccount", text.Text("IdSubAccount")),
            new WholeRule("IdRazdel", text.Text("IdRazdel")),
            new WholeRule("IdObject", text.Text("IdObject")),
            new WholeRule("IdFiBalance", text.Text("IdFiBalance")),
            new WholeRule("IdBalanceGroup", text.Text("IdBalanceGroup")),
            new RealRule("AssetsPercent", text.Text("AssetsPercent")),
            new RealRule("PSTNKD", text.Text("PSTNKD")),
            new FlagRule("IsMoney", text.Text("IsMoney")),
            new FlagRule("IsRur", text.Text("IsRur")),
            new RealRule("UchPrice", text.Text("UchPrice")),
            new RealRule("TorgPos", text.Text("TorgPos")),
            new RealRule("Price", text.Text("Price")),
            new RealRule("DailyPL", text.Text("DailyPL")),
            new RealRule("DailyPLPercentToMarketCurPrice", text.Text("DailyPLPercentToMarketCurPrice")),
            new RealRule("BackPos", text.Text("BackPos")),
            new RealRule("PrevQuote", text.Text("PrevQuote")),
            new RealRule("TrnIn", text.Text("TrnIn")),
            new RealRule("TrnOut", text.Text("TrnOut")),
            new RealRule("DailyBuyVolume", text.Text("DailyBuyVolume")),
            new RealRule("DailySellVolume", text.Text("DailySellVolume")),
            new RealRule("DailyBuyQuantity", text.Text("DailyBuyQuantity")),
            new RealRule("DailySellQuantity", text.Text("DailySellQuantity")),
            new RealRule("NKD", text.Text("NKD")),
            new RealRule("PriceStep", text.Text("PriceStep")),
            new WholeRule("Lot", text.Text("Lot")),
            new RealRule("NPLtoMarketCurPrice", text.Text("NPLtoMarketCurPrice")),
            new RealRule("NPLPercent", text.Text("NPLPercent")),
            new RealRule("PlanLong", text.Text("PlanLong")),
            new RealRule("PlanShort", text.Text("PlanShort"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the position element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
