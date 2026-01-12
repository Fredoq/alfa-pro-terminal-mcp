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
    /// <summary>
    /// Initializes a new ObjectTypeEntity with Type set to "ObjectTypeEntity" and Init set to true.
    /// </summary>
    public ObjectTypeEntity() : this("ObjectTypeEntity", true)
    {
    }

    /// <summary>
    /// Creates ObjectTypeEntity payload with explicit values. Usage example: var payload = new ObjectTypeEntity();.
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <summary>
    /// Initializes a new ObjectTypeEntity with the specified payload type and initialization flag.
    /// </summary>
    /// <param name="type">The payload type name to store.</param>
    /// <param name="init">`true` if this entity represents an initialization payload; `false` otherwise.</param>
    private ObjectTypeEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// <summary>
/// Serialize the payload into a JSON string containing the payload type and initialization flag.
/// </summary>
/// <returns>The JSON string with properties <c>Type</c> (payload type name) and <c>Init</c> (initialization flag).</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}