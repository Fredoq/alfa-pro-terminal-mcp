using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Descriptions;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;

/// <summary>
/// Provides description texts for position outputs. Usage example: string text = new PositionDescriptions().Text("IdPosition").
/// </summary>
internal sealed class PositionDescriptions : IDescriptions
{
    private readonly Dictionary<string, string> _data = new()
    {
        ["IdPosition"] = "Position identifier",
        ["IdAccount"] = "Client account id",
        ["IdSubAccount"] = "Client subaccount id",
        ["IdRazdel"] = "Portfolio id",
        ["IdObject"] = "Security identifier",
        ["IdFiBalance"] = "Financial instrument used for valuation",
        ["IdBalanceGroup"] = "Portfolio group identifier",
        ["AssetsPercent"] = "Position share in subaccount percent",
        ["PSTNKD"] = "Accrued coupon income",
        ["IsMoney"] = "Indicates money position",
        ["IsRur"] = "Indicates ruble currency position",
        ["UchPrice"] = "Accounting price",
        ["TorgPos"] = "Current position size",
        ["Price"] = "Current price",
        ["DailyPL"] = "Daily profit or loss",
        ["DailyPLPercentToMarketCurPrice"] = "Daily PnL percent to market price",
        ["BackPos"] = "Opening position",
        ["PrevQuote"] = "Previous session close price",
        ["TrnIn"] = "External credit volume",
        ["TrnOut"] = "External debit volume",
        ["DailyBuyVolume"] = "Session buy volume",
        ["DailySellVolume"] = "Session sell volume",
        ["DailyBuyQuantity"] = "Session buy quantity",
        ["DailySellQuantity"] = "Session sell quantity",
        ["NKD"] = "Accrued coupon income amount",
        ["PriceStep"] = "Price step",
        ["Lot"] = "Lot size",
        ["NPLtoMarketCurPrice"] = "Nominal profit or loss",
        ["NPLPercent"] = "Nominal profit or loss percent",
        ["PlanLong"] = "Planned long position",
        ["PlanShort"] = "Planned short position"
    };

    /// <summary>
    /// Returns a description text for the output field. Usage example: string text = descriptions.Text("Price").
    /// </summary>
    public string Text(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        if (_data.TryGetValue(name, out string? text))
        {
            return text;
        }
        throw new InvalidOperationException($"{name} description is missing");
    }
}
