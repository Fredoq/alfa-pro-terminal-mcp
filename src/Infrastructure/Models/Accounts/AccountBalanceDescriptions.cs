using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Provides description texts for account balance outputs. Usage example: string text = new AccountBalanceDescriptions().Text("Money").
/// </summary>
internal sealed class AccountBalanceDescriptions : IDescriptions
{
    private readonly Dictionary<string, string> _data = new()
    {
        ["DataId"] = "Balance identifier computed as IdSubAccount * 8 + IdRazdelGroup",
        ["IdAccount"] = "Client account id",
        ["IdSubAccount"] = "Client subaccount id",
        ["IdRazdelGroup"] = "Portfolio group code",
        ["MarginInitial"] = "Initial margin",
        ["MarginMinimum"] = "Minimum margin",
        ["MarginRequirement"] = "Margin requirements",
        ["Money"] = "Cash in rubles",
        ["MoneyInitial"] = "Opening cash in rubles",
        ["Balance"] = "Balance value",
        ["PrevBalance"] = "Opening balance",
        ["PortfolioCost"] = "Portfolio value",
        ["LiquidBalance"] = "Liquid portfolio value",
        ["Requirements"] = "Requirements",
        ["ImmediateRequirements"] = "Immediate requirements",
        ["NPL"] = "Nominal profit or loss",
        ["DailyPL"] = "Daily profit or loss",
        ["NPLPercent"] = "Nominal PnL percent",
        ["DailyPLPercent"] = "Daily PnL percent",
        ["NKD"] = "Accrued coupon income"
    };

    /// <summary>
    /// Returns a description text for the output field. Usage example: string text = descriptions.Text("Balance").
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
