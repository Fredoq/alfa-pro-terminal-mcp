using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the ClientBalanceEntity payload. Usage example: var payload = new ClientBalanceEntity(); string json = payload.AsString();.
/// </summary>
public sealed record ClientBalanceEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    public ClientBalanceEntity() : this("ClientBalanceEntity", true)
    {
    }

    private ClientBalanceEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString();.
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new
    {
        Type = _type,
        Init = _init
    });
}
