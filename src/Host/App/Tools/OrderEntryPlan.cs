using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for order entry. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class OrderEntryPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idSubAccount":{"type":"integer","description":"Client subaccount identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"},"idAllowedOrderParams":{"type":"integer","description":"Allowed order parameters identifier"}},"required":["idAccount","idSubAccount","idRazdel","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment","idAllowedOrderParams"],"additionalProperties":false}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orderEntry":{"type":"object","description":"Order entry response","properties":{"ResponseStatus":{"type":"integer","description":"Response status: 0 for OK, otherwise error"},"Message":{"type":"string","description":"Response status message"},"Error":{"type":"object","description":"Response error details","properties":{"Code":{"type":"integer","description":"Error code"},"Message":{"type":"string","description":"Error message"}},"required":["Code","Message"],"additionalProperties":false},"Value":{"type":"object","description":"Order entry response data","properties":{"ClientOrderNum":{"type":"integer","description":"Client order number"},"NumEDocument":{"type":"integer","description":"Broker order identifier"},"ErrorCode":{"type":"integer","description":"Terminal error code"},"ErrorText":{"type":"string","description":"Terminal error text"}},"required":["ClientOrderNum","NumEDocument","ErrorCode","ErrorText"],"additionalProperties":false}},"required":["ResponseStatus","Message","Error","Value"],"additionalProperties":false}},"required":["orderEntry"],"additionalProperties":false}""");
        return new Tool { Name = "order-enter", Title = "Order entry", Description = "Places a new order and returns the broker response.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = false, IdempotentHint = false, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idSubAccount":{"type":"integer","description":"Client subaccount identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"},"idAllowedOrderParams":{"type":"integer","description":"Allowed order parameters identifier"}},"required":["idAccount","idSubAccount","idRazdel","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment","idAllowedOrderParams"],"additionalProperties":false}"""));
        return new MappedPayload(data, schema);
    }
}
