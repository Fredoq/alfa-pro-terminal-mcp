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
    private readonly IReadOnlyDictionary<string, JsonElement> _extra;

    /// <summary>
    /// Creates mapped payload plan using the input schema. Usage example: IPayloadPlan plan = new MappedPayloadPlan(schema).
    /// </summary>
    /// <param name="schema">Input schema validator.</param>
    public MappedPayloadPlan(IInputSchema schema) : this(schema, new Dictionary<string, JsonElement>(StringComparer.Ordinal))
    {
    }

    /// <summary>
    /// Creates mapped payload plan using the input schema and extra values. Usage example: IPayloadPlan plan = new MappedPayloadPlan(schema, extra).
    /// </summary>
    /// <param name="schema">Input schema validator.</param>
    /// <param name="extra">Extra argument dictionary.</param>
    public MappedPayloadPlan(IInputSchema schema, IReadOnlyDictionary<string, JsonElement> extra)
    {
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(extra);
        _schema = schema;
        _extra = extra;
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data) => new MappedPayload(data, _schema, _extra);
}
