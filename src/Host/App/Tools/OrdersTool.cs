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
/// Provides MCP tool metadata and execution for current orders. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class OrdersTool : IMcpTool
{
    private readonly IOrders _orders;

    /// <summary>
    /// Creates orders tool with provided orders implementation. Usage example: IMcpTool tool = new OrdersTool(orders).
    /// </summary>
    /// <param name="orders">Orders provider.</param>
    public OrdersTool(IOrders orders)
    {
        _orders = orders;
    }

    /// <summary>
    /// Creates orders tool. Usage example: IMcpTool tool = new OrdersTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public OrdersTool(ITerminal terminal, ILogger logger)
        : this(new WsOrders(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "orders";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orders":{"type":"array","description":"Current orders for the requested account","items":{"type":"object","properties":{"NumEDocument":{"type":"integer","description":"Order identifier"},"ClientOrderNum":{"type":"integer","description":"Client order number"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdel":{"type":"integer","description":"Client subaccount portfolio id"},"IdAllowedOrderParams":{"type":"integer","description":"Order parameter combination identifier"},"AcceptTime":{"type":"string","description":"Order acceptance time"},"IdOrderType":{"type":"integer","description":"Order type identifier"},"IdObject":{"type":"integer","description":"Security identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"LimitPrice":{"type":"number","description":"Limit order price"},"BuySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"Quantity":{"type":"integer","description":"Quantity in units"},"Comment":{"type":"string","description":"Order comment"},"Login":{"type":"string","description":"Initiator login"},"IdOrderStatus":{"type":"integer","description":"Order status identifier"},"Rest":{"type":"integer","description":"Remaining quantity"},"Price":{"type":"number","description":"Order price"},"BrokerComment":{"type":"string","description":"Broker comment"}},"required":["NumEDocument","ClientOrderNum","IdAccount","IdSubAccount","IdRazdel","IdAllowedOrderParams","AcceptTime","IdOrderType","IdObject","IdMarketBoard","LimitPrice","BuySell","Quantity","Comment","Login","IdOrderStatus","Rest","Price","BrokerComment"],"additionalProperties":false}}},"required":["orders"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Current orders", Description = "Returns current orders for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("accountId", out _))
        {
            throw new McpProtocolException("Missing required argument accountId", McpErrorCode.InvalidParams);
        }
        JsonNode node = (await _orders.Entries(data["accountId"].GetInt64(), token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
