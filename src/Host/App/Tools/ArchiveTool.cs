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
/// Provides MCP tool metadata and execution for archive candles. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class ArchiveTool : IMcpTool
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates archive candles tool. Usage example: IMcpTool tool = new ArchiveTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public ArchiveTool(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "history";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"candles":{"type":"array","description":"Archive candles for the requested instrument and interval","items":{"oneOf":[{"type":"object","properties":{"Open":{"type":"number","description":"Opening price"},"Close":{"type":"number","description":"Closing price"},"Low":{"type":"number","description":"Lowest price in timeframe"},"High":{"type":"number","description":"Highest price in timeframe"},"Volume":{"type":"integer","description":"Traded volume in timeframe"},"VolumeAsk":{"type":"integer","description":"Ask volume in timeframe"},"OpenInt":{"type":"integer","description":"Open interest for futures"},"Time":{"type":"string","description":"Candle timestamp"}},"required":["Open","Close","Low","High","Volume","VolumeAsk","OpenInt","Time"],"additionalProperties":false},{"type":"object","properties":{"Open":{"type":"number","description":"Opening price"},"Close":{"type":"number","description":"Closing price"},"Time":{"type":"string","description":"Candle timestamp"},"Levels":{"type":"array","description":"Price levels for MPV candle","items":{"type":"object","properties":{"Price":{"type":"number","description":"Price at level"},"Volume":{"type":"integer","description":"Volume at level in timeframe"},"VolumeAsk":{"type":"integer","description":"Ask volume at level in timeframe"}},"required":["Price","Volume","VolumeAsk"],"additionalProperties":false}}},"required":["Open","Close","Time","Levels"],"additionalProperties":false}]}}},"required":["candles"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Archive candles", Description = "Returns archive candles for given instrument, candle type, interval, period, first day and last day.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("idFi", out JsonElement item))
        {
            throw new McpProtocolException("Missing required argument idFi", McpErrorCode.InvalidParams);
        }
        long id = item.GetInt64();
        if (!data.TryGetValue("candleType", out JsonElement type))
        {
            throw new McpProtocolException("Missing required argument candleType", McpErrorCode.InvalidParams);
        }
        int kind = type.GetInt32();
        if (!data.TryGetValue("interval", out JsonElement part))
        {
            throw new McpProtocolException("Missing required argument interval", McpErrorCode.InvalidParams);
        }
        string unit = part.GetString() ?? throw new McpProtocolException("Interval value is missing", McpErrorCode.InvalidParams);
        if (!data.TryGetValue("period", out JsonElement step))
        {
            throw new McpProtocolException("Missing required argument period", McpErrorCode.InvalidParams);
        }
        int span = step.GetInt32();
        if (!data.TryGetValue("firstDay", out JsonElement start))
        {
            throw new McpProtocolException("Missing required argument firstDay", McpErrorCode.InvalidParams);
        }
        DateTime begin = start.GetDateTime();
        if (!data.TryGetValue("lastDay", out JsonElement end))
        {
            throw new McpProtocolException("Missing required argument lastDay", McpErrorCode.InvalidParams);
        }
        DateTime finish = end.GetDateTime();
        WsArchive tool = new(_terminal, _logger);
        IEntries entries = await tool.History(id, kind, unit, span, begin, finish, token);
        JsonNode node = new RootEntries(entries, "candles").StructuredContent();
        string text = node.ToJsonString();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = text }] };
    }
}
