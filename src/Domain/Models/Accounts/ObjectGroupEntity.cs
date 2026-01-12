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
    /// <summary>
    /// Initializes a new instance of ObjectGroupEntity with Type set to "ObjectGroupEntity" and Init set to true.
    /// </summary>
    public ObjectGroupEntity() : this("ObjectGroupEntity", true)
    {
    }

    /// <summary>
    /// Initializes ObjectGroupEntity with explicit values through a private constructor.
    /// </summary>
    /// <param name="type">Payload type name used by ObjectGroupEntity(string type, bool init).</param>
    /// <param name="init">Initialization flag used by ObjectGroupEntity(string type, bool init).</param>
    /// <remarks>
    /// This constructor is private and cannot be called externally; use the public ObjectGroupEntity constructor or factory methods instead.
    /// <summary>
    /// Initializes a new ObjectGroupEntity with the specified payload type name and initialization flag.
    /// </summary>
    /// <param name="type">The payload type name to include in the serialized output.</param>
    /// <param name="init">Indicates whether the payload represents an initialized entity.</param>
    private ObjectGroupEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// <summary>
/// Serializes this payload to a JSON string containing the Type and Init properties.
/// </summary>
/// <returns>A JSON string with properties Type (payload type name) and Init (initialization flag).</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}