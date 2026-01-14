using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Creates mapped payloads using the provided input schema. Usage example: IPayloadPlan plan = new MappedPayloadPlan(schema).
/// </summary>
internal sealed class MappedPayloadPlan : IPayloadPlan
{
    private readonly IInputSchema _schema;

    /// <summary>
    /// Creates mapped payload plan using the input schema. Usage example: IPayloadPlan plan = new MappedPayloadPlan(schema).
    /// </summary>
    /// <param name="schema">Input schema validator.</param>
    public MappedPayloadPlan(IInputSchema schema)
    {
        _schema = schema;
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data) => new MappedPayload(data, _schema);
}
