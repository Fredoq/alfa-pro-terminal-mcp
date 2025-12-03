using System.Text.Json;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Describes a filtering rule for asset payload nodes. Usage example: bool matched = filter.Filtered(node);.
/// </summary>
public interface IAssetFilter
{
    /// <summary>
    /// Determines whether the asset node satisfies the filter. Usage example: filter.Filtered(node).
    /// </summary>
    /// <param name="node">Asset payload element.</param>
    /// <returns>True when asset matches the filter.</returns>
    bool Filtered(JsonElement node);
}
