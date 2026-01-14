using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for current orders. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class OrdersPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orders":{"type":"array","description":"Current orders for the requested account","items":{"type":"object","properties":{"NumEDocument":{"type":"integer","description":"Order identifier"},"ClientOrderNum":{"type":"integer","description":"Client order number"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdel":{"type":"integer","description":"Client subaccount portfolio id"},"IdAllowedOrderParams":{"type":"integer","description":"Order parameter combination identifier"},"AcceptTime":{"type":"string","description":"Order acceptance time"},"IdOrderType":{"type":"integer","description":"Order type identifier"},"IdObject":{"type":"integer","description":"Security identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"LimitPrice":{"type":"number","description":"Limit order price"},"BuySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"Quantity":{"type":"integer","description":"Quantity in units"},"Comment":{"type":"string","description":"Order comment"},"Login":{"type":"string","description":"Initiator login"},"IdOrderStatus":{"type":"integer","description":"Order status identifier"},"Rest":{"type":"integer","description":"Remaining quantity"},"Price":{"type":"number","description":"Order price"},"BrokerComment":{"type":"string","description":"Broker comment"}},"required":["NumEDocument","ClientOrderNum","IdAccount","IdSubAccount","IdRazdel","IdAllowedOrderParams","AcceptTime","IdOrderType","IdObject","IdMarketBoard","LimitPrice","BuySell","Quantity","Comment","Login","IdOrderStatus","Rest","Price","BrokerComment"],"additionalProperties":false}}},"required":["orders"],"additionalProperties":false}""");
        return new Tool { Name = "orders", Title = "Current orders", Description = "Returns current orders for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
