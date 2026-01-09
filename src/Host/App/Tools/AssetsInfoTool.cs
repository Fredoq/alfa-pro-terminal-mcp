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
/// Provides MCP tool metadata and execution for asset info by identifiers. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class AssetsInfoTool : IMcpTool
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates asset info tool. Usage example: IMcpTool tool = new AssetsInfoTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public AssetsInfoTool(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "info";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idObjects":{"type":"array","description":"Collection of IdObject values to extract","items":{"type":"integer"}}},"required":["idObjects"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"assets":{"type":"array","description":"Asset info entries for requested identifiers","items":{"type":"object","properties":{"IdObject":{"type":"integer","description":"Asset identifier"},"Ticker":{"type":"string","description":"Exchange ticker"},"ISIN":{"type":"string","description":"International security identifier"},"Name":{"type":"string","description":"Asset name"},"Description":{"type":"string","description":"Asset description"},"Nominal":{"type":"number","description":"Nominal value"},"IdObjectType":{"type":"integer","description":"Asset type identifier"},"IdObjectGroup":{"type":"integer","description":"Asset group identifier"},"IdObjectBase":{"type":"integer","description":"Base asset identifier"},"IdObjectFaceUnit":{"type":"integer","description":"Face value currency identifier"},"MatDateObject":{"type":"string","description":"Expiration date of asset"},"Instruments":{"type":"array","description":"Trading instrument details","items":{"type":"object","properties":{"IdFi":{"type":"integer","description":"Financial instrument identifier"},"RCode":{"type":"string","description":"Portfolio code"},"IsLiquid":{"type":"boolean","description":"Liquidity flag"},"IdMarketBoard":{"type":"integer","description":"Market identifier"}},"required":["IdFi","RCode","IsLiquid","IdMarketBoard"],"additionalProperties":false}}},"required":["IdObject","Ticker","ISIN","Name","Description","Nominal","IdObjectType","IdObjectGroup","IdObjectBase","IdObjectFaceUnit","MatDateObject","Instruments"],"additionalProperties":false}}},"required":["assets"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Asset info by identifiers", Description = "Returns asset info list for the given object identifiers.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("idObjects", out JsonElement item))
        {
            throw new McpProtocolException("Missing required argument idObjects", McpErrorCode.InvalidParams);
        }
        List<long> list = [];
        foreach (JsonElement part in item.EnumerateArray())
        {
            list.Add(part.GetInt64());
        }
        WsAssetsInfo tool = new(_terminal, _logger);
        IEntries entries = await tool.Info(list, token);
        JsonNode node = new RootEntries(entries, "assets").StructuredContent();
        string text = node.ToJsonString();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = text }] };
    }
}
