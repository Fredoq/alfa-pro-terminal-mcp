using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the ClientPositionEntity payload. Usage example: var payload = new ClientPositionEntity(); string json = payload.AsString();.
/// </summary>
public sealed record ClientPositionEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    public ClientPositionEntity() : this("ClientPositionEntity", true)
    {
    }

    private ClientPositionEntity(string type, bool init)
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
