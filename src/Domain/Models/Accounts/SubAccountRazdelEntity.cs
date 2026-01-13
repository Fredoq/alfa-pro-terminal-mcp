using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the SubAccountRazdelEntity payload. Usage example: var payload = new SubAccountRazdelEntity(); string json = payload.AsString();.
/// </summary>
public sealed record SubAccountRazdelEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    /// <summary>
    /// Creates SubAccountRazdelEntity payload. Usage example: var payload = new SubAccountRazdelEntity();.
    /// </summary>
    public SubAccountRazdelEntity() : this("SubAccountRazdelEntity", true)
    {
    }

    /// <summary>
    /// Initializes SubAccountRazdelEntity with explicit values through a private constructor.
    /// </summary>
    /// <param name="type">Payload type name used by SubAccountRazdelEntity(string type, bool init).</param>
    /// <param name="init">Initialization flag used by SubAccountRazdelEntity(string type, bool init).</param>
    /// <remarks>
    /// This constructor is private and cannot be called externally; use the public SubAccountRazdelEntity constructor or factory methods instead.
    /// </remarks>
    private SubAccountRazdelEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}
