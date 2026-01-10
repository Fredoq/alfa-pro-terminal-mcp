using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the FinInfoParamsEntity payload. Usage example: var payload = new FinInfoParamsEntity([123]); string json = payload.AsString();.
/// </summary>
public sealed record FinInfoParamsEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;
    private readonly IReadOnlyCollection<long> _keys;

    /// <summary>
    /// Creates FinInfoParamsEntity payload. Usage example: var payload = new FinInfoParamsEntity([123]);.
    /// </summary>
    /// <param name="keys">Financial instrument identifiers.</param>
    public FinInfoParamsEntity(IReadOnlyCollection<long> keys) : this("FinInfoParamsEntity", true, keys)
    {
    }

    /// <summary>
    /// Creates FinInfoParamsEntity payload with explicit values. Usage example: var payload = new FinInfoParamsEntity([123]);.
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <param name="init">Initialization flag.</param>
    /// <param name="keys">Financial instrument identifiers.</param>
    private FinInfoParamsEntity(string type, bool init, IReadOnlyCollection<long> keys)
    {
        _type = type;
        _init = init;
        _keys = keys;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Keys = _keys, Init = _init });
}
