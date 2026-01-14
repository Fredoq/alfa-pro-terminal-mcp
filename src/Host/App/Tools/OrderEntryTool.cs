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
/// Provides MCP tool metadata and execution for order entry. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class OrderEntryTool : IMcpTool
{
    private readonly IOrderEntry _entry;

    /// <summary>
    /// Creates order entry tool with provided order entry implementation. Usage example: IMcpTool tool = new OrderEntryTool(entry).
    /// </summary>
    /// <param name="entry">Order entry provider.</param>
    public OrderEntryTool(IOrderEntry entry)
    {
        _entry = entry;
    }

    /// <summary>
    /// Creates order entry tool. Usage example: IMcpTool tool = new OrderEntryTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public OrderEntryTool(ITerminal terminal, ILogger logger)
        : this(new WsOrderEntry(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "order-enter";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idSubAccount":{"type":"integer","description":"Client subaccount identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"},"idAllowedOrderParams":{"type":"integer","description":"Allowed order parameters identifier"}},"required":["idAccount","idSubAccount","idRazdel","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment","idAllowedOrderParams"],"additionalProperties":false}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orderEntry":{"type":"object","description":"Order entry response","properties":{"ResponseStatus":{"type":"integer","description":"Response status: 0 for OK, otherwise error"},"Message":{"type":"string","description":"Response status message"},"Error":{"type":"object","description":"Response error details","properties":{"Code":{"type":"integer","description":"Error code"},"Message":{"type":"string","description":"Error message"}},"required":["Code","Message"],"additionalProperties":false},"Value":{"type":"object","description":"Order entry response data","properties":{"ClientOrderNum":{"type":"integer","description":"Client order number"},"NumEDocument":{"type":"integer","description":"Broker order identifier"},"ErrorCode":{"type":"integer","description":"Terminal error code"},"ErrorText":{"type":"string","description":"Terminal error text"}},"required":["ClientOrderNum","NumEDocument","ErrorCode","ErrorText"],"additionalProperties":false}},"required":["ResponseStatus","Message","Error","Value"],"additionalProperties":false}},"required":["orderEntry"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Order entry", Description = "Places a new order and returns the broker response.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = false, IdempotentHint = false, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("idAccount", out _))
        {
            throw new McpProtocolException("Missing required argument idAccount", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idSubAccount", out _))
        {
            throw new McpProtocolException("Missing required argument idSubAccount", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idRazdel", out _))
        {
            throw new McpProtocolException("Missing required argument idRazdel", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idPriceControlType", out _))
        {
            throw new McpProtocolException("Missing required argument idPriceControlType", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idObject", out _))
        {
            throw new McpProtocolException("Missing required argument idObject", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("limitPrice", out _))
        {
            throw new McpProtocolException("Missing required argument limitPrice", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("stopPrice", out _))
        {
            throw new McpProtocolException("Missing required argument stopPrice", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("limitLevelAlternative", out _))
        {
            throw new McpProtocolException("Missing required argument limitLevelAlternative", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("buySell", out _))
        {
            throw new McpProtocolException("Missing required argument buySell", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("quantity", out _))
        {
            throw new McpProtocolException("Missing required argument quantity", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("comment", out _))
        {
            throw new McpProtocolException("Missing required argument comment", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idAllowedOrderParams", out _))
        {
            throw new McpProtocolException("Missing required argument idAllowedOrderParams", McpErrorCode.InvalidParams);
        }
        long account = data["idAccount"].GetInt64();
        long subaccount = data["idSubAccount"].GetInt64();
        long razdel = data["idRazdel"].GetInt64();
        int control = data["idPriceControlType"].GetInt32();
        long asset = data["idObject"].GetInt64();
        double limit = data["limitPrice"].GetDouble();
        double trigger = data["stopPrice"].GetDouble();
        double alternative = data["limitLevelAlternative"].GetDouble();
        int side = data["buySell"].GetInt32();
        int quantity = data["quantity"].GetInt32();
        string comment = data["comment"].GetString() ?? throw new McpProtocolException("Comment value is missing", McpErrorCode.InvalidParams);
        long allowed = data["idAllowedOrderParams"].GetInt64();
        JsonNode node = (await _entry.Entry(account, subaccount, razdel, control, asset, limit, trigger, alternative, side, quantity, comment, allowed, token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
