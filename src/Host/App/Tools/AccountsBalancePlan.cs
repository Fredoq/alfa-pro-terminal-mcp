using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for account balance. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class AccountsBalancePlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"balances":{"type":"array","description":"Account balance entries for the requested account","items":{"type":"object","properties":{"DataId":{"type":"integer","description":"Balance identifier computed as IdSubAccount * 8 + IdRazdelGroup"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdelGroup":{"type":"integer","description":"Portfolio group code"},"MarginInitial":{"type":"number","description":"Initial margin"},"MarginMinimum":{"type":"number","description":"Minimum margin"},"MarginRequirement":{"type":"number","description":"Margin requirements"},"Money":{"type":"number","description":"Cash in rubles"},"MoneyInitial":{"type":"number","description":"Opening cash in rubles"},"Balance":{"type":"number","description":"Balance value"},"PrevBalance":{"type":"number","description":"Opening balance"},"PortfolioCost":{"type":"number","description":"Portfolio value"},"LiquidBalance":{"type":"number","description":"Liquid portfolio value"},"Requirements":{"type":"number","description":"Requirements"},"ImmediateRequirements":{"type":"number","description":"Immediate requirements"},"NPL":{"type":"number","description":"Nominal profit or loss"},"DailyPL":{"type":"number","description":"Daily profit or loss"},"NPLPercent":{"type":"number","description":"Nominal PnL percent"},"DailyPLPercent":{"type":"number","description":"Daily PnL percent"},"NKD":{"type":"number","description":"Accrued coupon income"}},"required":["DataId","IdAccount","IdSubAccount","IdRazdelGroup","MarginInitial","MarginMinimum","MarginRequirement","Money","MoneyInitial","Balance","PrevBalance","PortfolioCost","LiquidBalance","Requirements","ImmediateRequirements","NPL","DailyPL","NPLPercent","DailyPLPercent","NKD"],"additionalProperties":false}}},"required":["balances"],"additionalProperties":false}""");
        return new Tool { Name = "balance", Title = "Account balance", Description = "Returns account balance for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
