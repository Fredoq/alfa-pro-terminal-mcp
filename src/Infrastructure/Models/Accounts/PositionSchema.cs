using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

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
        JsonIntegerItem integer = new();
        JsonDoubleItem doubley = new();
        JsonBoolItem booly = new();
        _schema = new RulesSchema([
            new ValueRule<long>(integer, "IdPosition", text.Text("IdPosition")),
            new ValueRule<long>(integer, "IdAccount", text.Text("IdAccount")),
            new ValueRule<long>(integer, "IdSubAccount", text.Text("IdSubAccount")),
            new ValueRule<long>(integer, "IdRazdel", text.Text("IdRazdel")),
            new ValueRule<long>(integer, "IdObject", text.Text("IdObject")),
            new ValueRule<long>(integer, "IdFiBalance", text.Text("IdFiBalance")),
            new ValueRule<long>(integer, "IdBalanceGroup", text.Text("IdBalanceGroup")),
            new ValueRule<double>(doubley, "AssetsPercent", text.Text("AssetsPercent")),
            new ValueRule<double>(doubley, "PSTNKD", text.Text("PSTNKD")),
            new ValueRule<bool>(booly, "IsMoney", text.Text("IsMoney")),
            new ValueRule<bool>(booly, "IsRur", text.Text("IsRur")),
            new ValueRule<double>(doubley, "UchPrice", text.Text("UchPrice")),
            new ValueRule<double>(doubley, "TorgPos", text.Text("TorgPos")),
            new ValueRule<double>(doubley, "Price", text.Text("Price")),
            new ValueRule<double>(doubley, "DailyPL", text.Text("DailyPL")),
            new ValueRule<double>(doubley, "DailyPLPercentToMarketCurPrice", text.Text("DailyPLPercentToMarketCurPrice")),
            new ValueRule<double>(doubley, "BackPos", text.Text("BackPos")),
            new ValueRule<double>(doubley, "PrevQuote", text.Text("PrevQuote")),
            new ValueRule<double>(doubley, "TrnIn", text.Text("TrnIn")),
            new ValueRule<double>(doubley, "TrnOut", text.Text("TrnOut")),
            new ValueRule<double>(doubley, "DailyBuyVolume", text.Text("DailyBuyVolume")),
            new ValueRule<double>(doubley, "DailySellVolume", text.Text("DailySellVolume")),
            new ValueRule<double>(doubley, "DailyBuyQuantity", text.Text("DailyBuyQuantity")),
            new ValueRule<double>(doubley, "DailySellQuantity", text.Text("DailySellQuantity")),
            new ValueRule<double>(doubley, "NKD", text.Text("NKD")),
            new ValueRule<double>(doubley, "PriceStep", text.Text("PriceStep")),
            new ValueRule<long>(integer, "Lot", text.Text("Lot")),
            new ValueRule<double>(doubley, "NPLtoMarketCurPrice", text.Text("NPLtoMarketCurPrice")),
            new ValueRule<double>(doubley, "NPLPercent", text.Text("NPLPercent")),
            new ValueRule<double>(doubley, "PlanLong", text.Text("PlanLong")),
            new ValueRule<double>(doubley, "PlanShort", text.Text("PlanShort"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the position element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
