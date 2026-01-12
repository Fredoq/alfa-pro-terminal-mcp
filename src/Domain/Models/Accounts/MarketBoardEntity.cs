using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the MarketBoardEntity payload. Usage example: var payload = new MarketBoardEntity(); string json = payload.AsString();.
/// </summary>
public sealed record MarketBoardEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    /// <summary>
    /// Creates MarketBoardEntity payload. Usage example: var payload = new MarketBoardEntity();.
    /// <summary>
    /// Initializes a MarketBoardEntity with its Type set to "MarketBoardEntity" and Init set to true.
    /// </summary>
    public MarketBoardEntity() : this("MarketBoardEntity", true)
    {
    }

    /// <summary>
    /// Creates MarketBoardEntity payload with explicit values. Usage example: var payload = new MarketBoardEntity();.
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <summary>
    /// Initializes a new MarketBoardEntity with the specified type and initialization flag.
    /// </summary>
    /// <param name="type">Payload type identifier included in the serialized output.</param>
    /// <param name="init">If true, marks the entity as an initial payload; otherwise false.</param>
    private MarketBoardEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// <summary>
/// Serialize the entity's payload into a JSON string containing Type and Init fields.
/// </summary>
/// <returns>A JSON string with Type and Init properties representing the entity's payload.</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}