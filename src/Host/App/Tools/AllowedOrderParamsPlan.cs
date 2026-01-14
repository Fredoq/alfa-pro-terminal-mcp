using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for allowed order parameters. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class AllowedOrderParamsPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"allowedOrderParams":{"type":"array","description":"Allowed order parameter entries","items":{"type":"object","properties":{"IdAllowedOrderParams":{"type":"integer","description":"Allowed order parameter identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"IdOrderType":{"type":"integer","description":"Order type identifier"},"IdDocumentType":{"type":"integer","description":"Document type identifier"},"IdQuantityType":{"type":"integer","description":"Quantity type identifier"},"IdPriceType":{"type":"integer","description":"Price type identifier"},"IdLifeTime":{"type":"integer","description":"Order lifetime identifier"},"IdExecutionType":{"type":"integer","description":"Execution type identifier"}},"required":["IdAllowedOrderParams","IdObjectGroup","IdMarketBoard","IdOrderType","IdDocumentType","IdQuantityType","IdPriceType","IdLifeTime","IdExecutionType"],"additionalProperties":false}}},"required":["allowedOrderParams"],"additionalProperties":false}""");
        return new Tool { Name = "allowed-order-params", Title = "Allowed order parameters", Description = "Returns allowed order parameter entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        schema.Ensure(data);
        return new AllowedOrderParamEntity();
    }
}
