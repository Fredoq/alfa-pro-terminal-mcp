using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for account balance. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class AccountsBalanceTool : IMcpTool
{
    private readonly IBalances _balances;

    /// <summary>
    /// Creates account balance tool with provided balances implementation. Usage example: IMcpTool tool = new AccountsBalanceTool(balances).
    /// </summary>
    /// <param name="balances">Account balances provider.</param>
    public AccountsBalanceTool(IBalances balances)
    {
        _balances = balances;
    }

    /// <summary>
    /// Creates account balance tool. Usage example: IMcpTool tool = new AccountsBalanceTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public AccountsBalanceTool(ITerminal terminal, ILogger logger)
        : this(new WsBalance(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "balance";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"balances":{"type":"array","description":"Account balance entries for the requested account","items":{"type":"object","properties":{"DataId":{"type":"integer","description":"Balance identifier computed as IdSubAccount * 8 + IdRazdelGroup"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdelGroup":{"type":"integer","description":"Portfolio group code"},"MarginInitial":{"type":"number","description":"Initial margin"},"MarginMinimum":{"type":"number","description":"Minimum margin"},"MarginRequirement":{"type":"number","description":"Margin requirements"},"Money":{"type":"number","description":"Cash in rubles"},"MoneyInitial":{"type":"number","description":"Opening cash in rubles"},"Balance":{"type":"number","description":"Balance value"},"PrevBalance":{"type":"number","description":"Opening balance"},"PortfolioCost":{"type":"number","description":"Portfolio value"},"LiquidBalance":{"type":"number","description":"Liquid portfolio value"},"Requirements":{"type":"number","description":"Requirements"},"ImmediateRequirements":{"type":"number","description":"Immediate requirements"},"NPL":{"type":"number","description":"Nominal profit or loss"},"DailyPL":{"type":"number","description":"Daily profit or loss"},"NPLPercent":{"type":"number","description":"Nominal PnL percent"},"DailyPLPercent":{"type":"number","description":"Daily PnL percent"},"NKD":{"type":"number","description":"Accrued coupon income"}},"required":["DataId","IdAccount","IdSubAccount","IdRazdelGroup","MarginInitial","MarginMinimum","MarginRequirement","Money","MoneyInitial","Balance","PrevBalance","PortfolioCost","LiquidBalance","Requirements","ImmediateRequirements","NPL","DailyPL","NPLPercent","DailyPLPercent","NKD"],"additionalProperties":false}}},"required":["balances"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Account balance", Description = "Returns account balance for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("accountId", out JsonElement item))
        {
            throw new McpProtocolException("Missing required argument accountId", McpErrorCode.InvalidParams);
        }
        JsonNode node = (await _balances.Balance(item.GetInt64(), token)).StructuredContent();
        string text = node.ToJsonString();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = text }] };
    }
}
