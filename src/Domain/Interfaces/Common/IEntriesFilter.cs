using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

/// <summary>
/// Describes a filtering rule for entry nodes. Usage example: bool matched = filter.Filtered(node);.
/// </summary>
public interface IEntriesFilter
{
    /// <summary>
    /// Determines whether the node satisfies the filter. Usage example: filter.Filtered(node).
    /// </summary>
    /// <param name="node">Entry object.</param>
    /// <returns>True when the node matches the filter.</returns>
    bool Filtered(JsonObject node);
}
