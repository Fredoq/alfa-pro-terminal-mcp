using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for asset info by tickers. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class AssetsTickersTool : IMcpTool
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;
    private readonly IContent _content;

    /// <summary>
    /// Creates asset info tool by tickers. Usage example: IMcpTool tool = new AssetsTickersTool(terminal, logger, content).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="content">Response formatter.</param>
    public AssetsTickersTool(ITerminal terminal, ILogger logger, IContent content)
    {
        _terminal = terminal;
        _logger = logger;
        _content = content;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "infoByTickers";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"tickers":{"type":"array","description":"Collection of ticker symbols to extract","items":{"type":"string"}}},"required":["tickers"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"assets":{"type":"array","description":"Asset info entries for requested tickers","items":{"type":"object","properties":{"IdObject":{"type":"integer","description":"Asset identifier"},"Ticker":{"type":"string","description":"Exchange ticker"},"ISIN":{"type":"string","description":"International security identifier"},"Name":{"type":"string","description":"Asset name"},"Description":{"type":"string","description":"Asset description"},"Nominal":{"type":"number","description":"Nominal value"},"IdObjectType":{"type":"integer","description":"Asset type identifier"},"IdObjectGroup":{"type":"integer","description":"Asset group identifier"},"IdObjectBase":{"type":"integer","description":"Base asset identifier"},"IdObjectFaceUnit":{"type":"integer","description":"Face value currency identifier"},"MatDateObject":{"type":"string","description":"Expiration date of asset"},"Instruments":{"type":"array","description":"Trading instrument details","items":{"type":"object","properties":{"IdFi":{"type":"integer","description":"Financial instrument identifier"},"RCode":{"type":"string","description":"Portfolio code"},"IsLiquid":{"type":"boolean","description":"Liquidity flag"},"IdMarketBoard":{"type":"integer","description":"Market identifier"}},"required":["IdFi","RCode","IsLiquid","IdMarketBoard"],"additionalProperties":false}}},"required":["IdObject","Ticker","ISIN","Name","Description","Nominal","IdObjectType","IdObjectGroup","IdObjectBase","IdObjectFaceUnit","MatDateObject","Instruments"],"additionalProperties":false}}},"required":["assets"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Asset info by tickers", Description = "Returns asset info list for the given ticker symbols.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("tickers", out JsonElement item))
        {
            throw new McpProtocolException("Missing required argument tickers", McpErrorCode.InvalidParams);
        }
        List<string> list = [];
        foreach (JsonElement part in item.EnumerateArray())
        {
            string text = part.GetString() ?? throw new McpProtocolException("Ticker value is missing", McpErrorCode.InvalidParams);
            list.Add(text);
        }
        WsAssetsInfo tool = new(_terminal, _logger);
        IEntries entries = await tool.InfoByTickers(list, token);
        return _content.Result(entries, "assets");
    }
}
