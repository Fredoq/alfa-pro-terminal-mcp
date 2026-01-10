using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the ObjectGroupEntity payload. Usage example: var payload = new ObjectGroupEntity(); string json = payload.AsString();.
/// </summary>
public sealed record ObjectGroupEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    /// <summary>
    /// Creates ObjectGroupEntity payload. Usage example: var payload = new ObjectGroupEntity();.
    /// </summary>
    public ObjectGroupEntity() : this("ObjectGroupEntity", true)
    {
    }

    /// <summary>
    /// Creates ObjectGroupEntity payload with explicit values. Usage example: var payload = new ObjectGroupEntity();.
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <param name="init">Initialization flag.</param>
    private ObjectGroupEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}
