using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

/// <summary>
/// Represents the AssetInfoEntity payload. Usage example: var payload = new AssetInfoEntity(); string json = payload.AsString();.
/// </summary>
public sealed record AssetInfoEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    public AssetInfoEntity() : this("AssetInfoEntity", true)
    {
    }

    private AssetInfoEntity(string type, bool init)
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
