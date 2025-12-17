using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Defines an output schema for a single position entry. Usage example: JsonNode node = new PositionSchema().Node(element).
/// </summary>
internal sealed class PositionSchema : IJsonSchema
{
    /// <summary>
    /// Returns an output node for the position element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node)
    {
        PositionDescriptions text = new();
        RulesSchema schema = new([
            new ValueRule<long>(new JsonInteger(node, "IdPosition"), "IdPosition", text.Text("IdPosition")),
            new ValueRule<long>(new JsonInteger(node, "IdAccount"), "IdAccount", text.Text("IdAccount")),
            new ValueRule<long>(new JsonInteger(node, "IdSubAccount"), "IdSubAccount", text.Text("IdSubAccount")),
            new ValueRule<long>(new JsonInteger(node, "IdRazdel"), "IdRazdel", text.Text("IdRazdel")),
            new ValueRule<long>(new JsonInteger(node, "IdObject"), "IdObject", text.Text("IdObject")),
            new ValueRule<long>(new JsonInteger(node, "IdFiBalance"), "IdFiBalance", text.Text("IdFiBalance")),
            new ValueRule<long>(new JsonInteger(node, "IdBalanceGroup"), "IdBalanceGroup", text.Text("IdBalanceGroup")),
            new ValueRule<double>(new JsonDouble(node, "AssetsPercent"), "AssetsPercent", text.Text("AssetsPercent")),
            new ValueRule<double>(new JsonDouble(node, "PSTNKD"), "PSTNKD", text.Text("PSTNKD")),
            new ValueRule<bool>(new JsonBool(node, "IsMoney"), "IsMoney", text.Text("IsMoney")),
            new ValueRule<bool>(new JsonBool(node, "IsRur"), "IsRur", text.Text("IsRur")),
            new ValueRule<double>(new JsonDouble(node, "UchPrice"), "UchPrice", text.Text("UchPrice")),
            new ValueRule<double>(new JsonDouble(node, "TorgPos"), "TorgPos", text.Text("TorgPos")),
            new ValueRule<double>(new JsonDouble(node, "Price"), "Price", text.Text("Price")),
            new ValueRule<double>(new JsonDouble(node, "DailyPL"), "DailyPL", text.Text("DailyPL")),
            new ValueRule<double>(new JsonDouble(node, "DailyPLPercentToMarketCurPrice"), "DailyPLPercentToMarketCurPrice", text.Text("DailyPLPercentToMarketCurPrice")),
            new ValueRule<double>(new JsonDouble(node, "BackPos"), "BackPos", text.Text("BackPos")),
            new ValueRule<double>(new JsonDouble(node, "PrevQuote"), "PrevQuote", text.Text("PrevQuote")),
            new ValueRule<double>(new JsonDouble(node, "TrnIn"), "TrnIn", text.Text("TrnIn")),
            new ValueRule<double>(new JsonDouble(node, "TrnOut"), "TrnOut", text.Text("TrnOut")),
            new ValueRule<double>(new JsonDouble(node, "DailyBuyVolume"), "DailyBuyVolume", text.Text("DailyBuyVolume")),
            new ValueRule<double>(new JsonDouble(node, "DailySellVolume"), "DailySellVolume", text.Text("DailySellVolume")),
            new ValueRule<double>(new JsonDouble(node, "DailyBuyQuantity"), "DailyBuyQuantity", text.Text("DailyBuyQuantity")),
            new ValueRule<double>(new JsonDouble(node, "DailySellQuantity"), "DailySellQuantity", text.Text("DailySellQuantity")),
            new ValueRule<double>(new JsonDouble(node, "NKD"), "NKD", text.Text("NKD")),
            new ValueRule<double>(new JsonDouble(node, "PriceStep"), "PriceStep", text.Text("PriceStep")),
            new ValueRule<long>(new JsonInteger(node, "Lot"), "Lot", text.Text("Lot")),
            new ValueRule<double>(new JsonDouble(node, "NPLtoMarketCurPrice"), "NPLtoMarketCurPrice", text.Text("NPLtoMarketCurPrice")),
            new ValueRule<double>(new JsonDouble(node, "NPLPercent"), "NPLPercent", text.Text("NPLPercent")),
            new ValueRule<double>(new JsonDouble(node, "PlanLong"), "PlanLong", text.Text("PlanLong")),
            new ValueRule<double>(new JsonDouble(node, "PlanShort"), "PlanShort", text.Text("PlanShort"))
        ]);
        return schema.Node(node);
    }
}
