using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Returns a fixed payload after validating input schema. Usage example: IPayloadPlan plan = new FixedPayloadPlan(schema, payload).
/// </summary>
internal sealed class FixedPayloadPlan : IPayloadPlan
{
    private readonly IInputSchema _schema;
    private readonly IPayload _payload;

    /// <summary>
    /// Creates fixed payload plan using the input schema and payload. Usage example: IPayloadPlan plan = new FixedPayloadPlan(schema, payload).
    /// </summary>
    /// <param name="schema">Input schema validator.</param>
    /// <param name="payload">Payload instance.</param>
    public FixedPayloadPlan(IInputSchema schema, IPayload payload)
    {
        _schema = schema;
        _payload = payload;
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        _schema.Ensure(data);
        return _payload;
    }
}
