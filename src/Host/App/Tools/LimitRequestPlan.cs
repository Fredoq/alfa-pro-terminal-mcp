using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for limit requests. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class LimitRequestPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idObject":{"type":"integer","description":"Security identifier"},"idMarketBoard":{"type":"integer","description":"Market identifier"},"idDocumentType":{"type":"integer","description":"Document type identifier"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"price":{"type":"number","description":"Order price"},"idOrderType":{"type":"integer","description":"Order type identifier: 1 for market or 2 for limit"},"limitRequestType":{"type":"integer","description":"Requested limit type: 3 for free money or 4 for portfolio cost"}},"required":["idAccount","idRazdel","idObject","idMarketBoard","idDocumentType","buySell","price","idOrderType","limitRequestType"]}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"limit":{"type":"object","description":"Limit response for requested order parameters","properties":{"Quantity":{"type":"integer","description":"Available quantity"},"QuantityForOwnAssets":{"type":"integer","description":"Available quantity without leverage"}},"required":["Quantity","QuantityForOwnAssets"],"additionalProperties":false}},"required":["limit"],"additionalProperties":false}""");
        return new Tool { Name = "limit-request", Title = "Limit request", Description = "Returns available limit for the given order parameters.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idObject":{"type":"integer","description":"Security identifier"},"idMarketBoard":{"type":"integer","description":"Market identifier"},"idDocumentType":{"type":"integer","description":"Document type identifier"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"price":{"type":"number","description":"Order price"},"idOrderType":{"type":"integer","description":"Order type identifier: 1 for market or 2 for limit"},"limitRequestType":{"type":"integer","description":"Requested limit type: 3 for free money or 4 for portfolio cost"}},"required":["idAccount","idRazdel","idObject","idMarketBoard","idDocumentType","buySell","price","idOrderType","limitRequestType"]}"""));
        return new MappedPayload(data, schema);
    }
}
