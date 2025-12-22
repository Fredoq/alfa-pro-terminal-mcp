using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;

/// <summary>
/// Narrows asset payloads to specified identifiers. Usage example: bool matched = new AssetIdsScope(ids).Filtered(node);.
/// </summary>
internal sealed class AssetIdsScope : IEntriesFilter
{
    private readonly HashSet<long> _ids;

    /// <summary>
    /// Creates filter limited to provided identifiers. Usage example: new AssetIdsScope(ids).
    /// </summary>
    public AssetIdsScope(IEnumerable<long> ids)
    {
        ArgumentNullException.ThrowIfNull(ids);
        _ids = [.. ids];
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
        long id = new JsonInteger(node, "IdObject").Value();
        return _ids.Contains(id);
    }
}
