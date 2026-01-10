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
    /// </summary>
    public MarketBoardEntity() : this("MarketBoardEntity", true)
    {
    }

    /// <summary>
    /// Creates MarketBoardEntity payload with explicit values. Usage example: var payload = new MarketBoardEntity();.
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <param name="init">Initialization flag.</param>
    private MarketBoardEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Type = _type, Init = _init });
}
