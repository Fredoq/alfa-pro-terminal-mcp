using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the ClientSubAccountEntity payload. Usage example: var payload = new ClientSubAccountEntity(); string json = payload.AsString();.
/// </summary>
public sealed record ClientSubAccountEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    /// <summary>
    /// Creates ClientSubAccountEntity payload. Usage example: var payload = new ClientSubAccountEntity();.
    /// </summary>
    public ClientSubAccountEntity() : this("ClientSubAccountEntity", true)
    {
    }

    /// <summary>
    /// Initializes ClientSubAccountEntity with explicit values through a private constructor.
    /// </summary>
    /// <param name="type">Payload type name used by ClientSubAccountEntity(string type, bool init).</param>
    /// <param name="init">Initialization flag used by ClientSubAccountEntity(string type, bool init).</param>
    /// <remarks>
    /// This constructor is private and cannot be called externally; use the public ClientSubAccountEntity constructor or factory methods instead.
    /// </remarks>
    private ClientSubAccountEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}
