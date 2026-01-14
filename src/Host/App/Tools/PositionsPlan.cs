using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for account positions. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class PositionsPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"positions":{"type":"array","description":"Account positions for the requested account","items":{"type":"object","properties":{"IdPosition":{"type":"integer","description":"Position identifier"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdel":{"type":"integer","description":"Portfolio id"},"IdObject":{"type":"integer","description":"Security identifier"},"IdFiBalance":{"type":"integer","description":"Financial instrument used for valuation"},"IdBalanceGroup":{"type":"integer","description":"Portfolio group identifier"},"AssetsPercent":{"type":"number","description":"Position share in subaccount percent"},"PSTNKD":{"type":"number","description":"Accrued coupon income"},"IsMoney":{"type":"boolean","description":"Indicates money position"},"IsRur":{"type":"boolean","description":"Indicates ruble currency position"},"UchPrice":{"type":"number","description":"Accounting price"},"TorgPos":{"type":"number","description":"Current position size"},"Price":{"type":"number","description":"Current price"},"DailyPL":{"type":"number","description":"Daily profit or loss"},"DailyPLPercentToMarketCurPrice":{"type":"number","description":"Daily PnL percent to market price"},"BackPos":{"type":"number","description":"Opening position"},"PrevQuote":{"type":"number","description":"Previous session close price"},"TrnIn":{"type":"number","description":"External credit volume"},"TrnOut":{"type":"number","description":"External debit volume"},"DailyBuyVolume":{"type":"number","description":"Session buy volume"},"DailySellVolume":{"type":"number","description":"Session sell volume"},"DailyBuyQuantity":{"type":"number","description":"Session buy quantity"},"DailySellQuantity":{"type":"number","description":"Session sell quantity"},"NKD":{"type":"number","description":"Accrued coupon income amount"},"PriceStep":{"type":"number","description":"Price step"},"Lot":{"type":"integer","description":"Lot size"},"NPLtoMarketCurPrice":{"type":"number","description":"Nominal profit or loss"},"NPLPercent":{"type":"number","description":"Nominal profit or loss percent"},"PlanLong":{"type":"number","description":"Planned long position"},"PlanShort":{"type":"number","description":"Planned short position"}},"required":["IdPosition","IdAccount","IdSubAccount","IdRazdel","IdObject","IdFiBalance","IdBalanceGroup","AssetsPercent","PSTNKD","IsMoney","IsRur","UchPrice","TorgPos","Price","DailyPL","DailyPLPercentToMarketCurPrice","BackPos","PrevQuote","TrnIn","TrnOut","DailyBuyVolume","DailySellVolume","DailyBuyQuantity","DailySellQuantity","NKD","PriceStep","Lot","NPLtoMarketCurPrice","NPLPercent","PlanLong","PlanShort"],"additionalProperties":false}}},"required":["positions"],"additionalProperties":false}""");
        return new Tool { Name = "positions", Title = "Account positions", Description = "Returns positions for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""));
        return new MappedPayload(data, schema);
    }
}
