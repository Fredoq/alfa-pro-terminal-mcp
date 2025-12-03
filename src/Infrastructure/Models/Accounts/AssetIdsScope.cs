using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Narrows asset payloads to specified identifiers. Usage example: bool matched = new AssetIdsScope(ids).Filtered(node);.
/// </summary>
internal sealed class AssetIdsScope : IAssetFilter
{
    private readonly HashSet<long> _ids;

    /// <summary>
    /// Creates filter limited to provided identifiers. Usage example: new AssetIdsScope(ids).
    /// </summary>
    public AssetIdsScope(IEnumerable<long> ids)
    {
        ArgumentNullException.ThrowIfNull(ids);
        _ids = new HashSet<long>(ids);
        if (_ids.Count == 0)
        {
            throw new InvalidOperationException("Asset identifiers list is empty");
        }
    }

    /// <summary>
    /// Checks whether payload matches identifier list. Usage example: scope.Filtered(node).
    /// </summary>
    public bool Filtered(JsonElement node)
    {
        long id = Id(node);
        return _ids.Contains(id);
    }

    /// <summary>
    /// Extracts identifier from payload. Usage example: long id = Id(node);.
    /// </summary>
    private static long Id(JsonElement node)
    {
        if (!node.TryGetProperty("IdObject", out JsonElement value))
        {
            throw new InvalidOperationException("IdObject is missing");
        }
        if (value.ValueKind != JsonValueKind.Number)
        {
            throw new InvalidOperationException("IdObject is missing");
        }
        return value.GetInt64();
    }
}
