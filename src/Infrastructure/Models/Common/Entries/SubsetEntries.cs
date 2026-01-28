using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Returns a subset of JSON array entries by a predicate. Usage example: JsonNode node = new SubsetEntries(entries, filter).StructuredContent().
/// </summary>
internal sealed class SubsetEntries : IEntries
{
    private readonly IEntries _entries;
    private readonly IEntriesFilter _filter;

    /// <summary>
    /// Creates subset behavior for entries. Usage example: var entries = new SubsetEntries(inner, filter).
    /// </summary>
    /// <param name="entries">Source entries.</param>
    /// <param name="filter">Node filter.</param>
    public SubsetEntries(IEntries entries, IEntriesFilter filter)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(filter);
        _entries = entries;
        _filter = filter;
    }

    /// <summary>
    /// Returns subset entries as structured content. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent()
    {
        JsonNode node = _entries.StructuredContent();
        JsonArray array;
        try
        {
            array = node.AsArray();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("Entries array is missing");
        }
        JsonArray list = [];
        foreach (JsonNode? item in array)
        {
            if (item is null)
            {
                throw new InvalidOperationException("Entry node is missing");
            }
            if (!_filter.Filtered(item.AsObject()))
            {
                continue;
            }
            list.Add(item.DeepClone());
        }
        return list;
    }
}
