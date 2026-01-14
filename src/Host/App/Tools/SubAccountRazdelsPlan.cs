using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for subaccount portfolios. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class SubAccountRazdelsPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"subAccountRazdels":{"type":"array","description":"Subaccount portfolio entries","items":{"type":"object","properties":{"IdRazdel":{"type":"integer","description":"Portfolio identifier"},"IdAccount":{"type":"integer","description":"Client account identifier"},"IdSubAccount":{"type":"integer","description":"Client subaccount identifier"},"IdRazdelGroup":{"type":"integer","description":"Portfolio group identifier"},"RCode":{"type":"string","description":"Portfolio code"}},"required":["IdRazdel","IdAccount","IdSubAccount","IdRazdelGroup","RCode"],"additionalProperties":false}}},"required":["subAccountRazdels"],"additionalProperties":false}""");
        return new Tool { Name = "subaccount-razdels", Title = "Subaccount portfolios", Description = "Returns subaccount portfolio entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
        return new SubAccountRazdelEntity();
    }
}
