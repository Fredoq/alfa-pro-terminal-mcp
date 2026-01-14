using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

/// <summary>
/// Represents a typed payload envelope for routing requests. Usage example: IPayload payload = new EntityPayload("ClientBalanceEntity", true).
/// </summary>
public sealed record EntityPayload : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    /// <summary>
    /// Creates payload envelope with explicit values. Usage example: var payload = new EntityPayload("OrderEntity", true).
    /// </summary>
    /// <param name="type">Payload type name.</param>
    /// <param name="init">Initialization flag.</param>
    public EntityPayload(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    /// <summary>
    /// Serializes payload into a string. Usage example: string json = payload.AsString().
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString() => JsonSerializer.Serialize(new { Type = _type, Init = _init });
}
