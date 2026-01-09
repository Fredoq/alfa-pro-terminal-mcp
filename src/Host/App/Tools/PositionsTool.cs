using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for account positions. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class PositionsTool : IMcpTool
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates account positions tool. Usage example: IMcpTool tool = new PositionsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public PositionsTool(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "positions";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"positions":{"type":"array","description":"Account positions for the requested account","items":{"type":"object","properties":{"IdPosition":{"type":"integer","description":"Position identifier"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdel":{"type":"integer","description":"Portfolio id"},"IdObject":{"type":"integer","description":"Security identifier"},"IdFiBalance":{"type":"integer","description":"Financial instrument used for valuation"},"IdBalanceGroup":{"type":"integer","description":"Portfolio group identifier"},"AssetsPercent":{"type":"number","description":"Position share in subaccount percent"},"PSTNKD":{"type":"number","description":"Accrued coupon income"},"IsMoney":{"type":"boolean","description":"Indicates money position"},"IsRur":{"type":"boolean","description":"Indicates ruble currency position"},"UchPrice":{"type":"number","description":"Accounting price"},"TorgPos":{"type":"number","description":"Current position size"},"Price":{"type":"number","description":"Current price"},"DailyPL":{"type":"number","description":"Daily profit or loss"},"DailyPLPercentToMarketCurPrice":{"type":"number","description":"Daily PnL percent to market price"},"BackPos":{"type":"number","description":"Opening position"},"PrevQuote":{"type":"number","description":"Previous session close price"},"TrnIn":{"type":"number","description":"External credit volume"},"TrnOut":{"type":"number","description":"External debit volume"},"DailyBuyVolume":{"type":"number","description":"Session buy volume"},"DailySellVolume":{"type":"number","description":"Session sell volume"},"DailyBuyQuantity":{"type":"number","description":"Session buy quantity"},"DailySellQuantity":{"type":"number","description":"Session sell quantity"},"NKD":{"type":"number","description":"Accrued coupon income amount"},"PriceStep":{"type":"number","description":"Price step"},"Lot":{"type":"integer","description":"Lot size"},"NPLtoMarketCurPrice":{"type":"number","description":"Nominal profit or loss"},"NPLPercent":{"type":"number","description":"Nominal profit or loss percent"},"PlanLong":{"type":"number","description":"Planned long position"},"PlanShort":{"type":"number","description":"Planned short position"}},"required":["IdPosition","IdAccount","IdSubAccount","IdRazdel","IdObject","IdFiBalance","IdBalanceGroup","AssetsPercent","PSTNKD","IsMoney","IsRur","UchPrice","TorgPos","Price","DailyPL","DailyPLPercentToMarketCurPrice","BackPos","PrevQuote","TrnIn","TrnOut","DailyBuyVolume","DailySellVolume","DailyBuyQuantity","DailySellQuantity","NKD","PriceStep","Lot","NPLtoMarketCurPrice","NPLPercent","PlanLong","PlanShort"],"additionalProperties":false}}},"required":["positions"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Account positions", Description = "Returns positions for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
        long id = item.GetInt64();
        WsPositions tool = new(_terminal, _logger);
        IEntries entries = await tool.Entries(id, token);
        JsonNode node = new RootEntries(entries, "positions").StructuredContent();
        string text = node.ToJsonString();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = text }] };
    }
}
