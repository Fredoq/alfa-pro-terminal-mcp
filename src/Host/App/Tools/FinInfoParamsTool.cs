using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for fin info params entries. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class FinInfoParamsTool : IMcpTool
{
    private readonly IFinInfoParams _info;

    /// <summary>
    /// Creates fin info params tool with provided entries implementation. Usage example: IMcpTool tool = new FinInfoParamsTool(info).
    /// </summary>
    /// <param name="info">Fin info params entries provider.</param>
    public FinInfoParamsTool(IFinInfoParams info)
    {
        _info = info;
    }

    /// <summary>
    /// Creates fin info params tool. Usage example: IMcpTool tool = new FinInfoParamsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public FinInfoParamsTool(ITerminal terminal, ILogger logger)
        : this(new WsFinInfoParams(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "fin-info-params";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"}},"required":["idFi"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"finInfoParams":{"type":"array","description":"Trading parameters for the requested instrument","items":{"type":"object","properties":{"IdFi":{"type":"integer","description":"Financial instrument identifier"},"IdSession":{"type":"integer","description":"Session identifier"},"IdTradePeriodStatus":{"type":"integer","description":"Trade status identifier"},"SessionDate":{"type":"string","description":"Session date"},"Lot":{"type":"integer","description":"Units per lot"},"PriceStep":{"type":"number","description":"Price step"},"PriceStepCost":{"type":"number","description":"Price step cost"},"IdObjectCurrency":{"type":"integer","description":"Price currency object identifier"},"PSTNKD":{"type":"number","description":"Accrued coupon income"},"UpPrice":{"type":"number","description":"Upper price limit"},"DownPrice":{"type":"number","description":"Lower price limit"},"GtBuy":{"type":"number","description":"Initial margin for buy"},"GtSell":{"type":"number","description":"Initial margin for sell"},"FaceValue":{"type":"number","description":"Nominal value"}},"required":["IdFi","IdSession","IdTradePeriodStatus","SessionDate","Lot","PriceStep","PriceStepCost","IdObjectCurrency","PSTNKD","UpPrice","DownPrice","GtBuy","GtSell","FaceValue"],"additionalProperties":false}}},"required":["finInfoParams"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Fin info params", Description = "Returns trading parameters for the given instrument.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("idFi", out _))
        {
            throw new McpProtocolException("Missing required argument idFi", McpErrorCode.InvalidParams);
        }
        long id = data["idFi"].GetInt64();
        JsonNode node = (await _info.Entries(id, token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
