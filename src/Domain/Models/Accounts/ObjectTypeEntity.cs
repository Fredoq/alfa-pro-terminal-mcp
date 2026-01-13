using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the ObjectTypeEntity payload. Usage example: var payload = new ObjectTypeEntity(); string json = payload.AsString();.
/// </summary>
public sealed record ObjectTypeEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    /// <summary>
    /// Creates ObjectTypeEntity payload. Usage example: var payload = new ObjectTypeEntity();.
    /// </summary>
    public ObjectTypeEntity() : this("ObjectTypeEntity", true)
    {
    }

    /// <summary>
    /// Creates ObjectTypeEntity payload with explicit values. Usage example: var payload = new ObjectTypeEntity();.
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <param name="init">Initialization flag.</param>
    private ObjectTypeEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}
