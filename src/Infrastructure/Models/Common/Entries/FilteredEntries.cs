using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Filters JSON array entries by a predicate. Usage example: JsonNode node = new FilteredEntries(entries, filter, "Entries are missing").StructuredContent().
/// </summary>
internal sealed class FilteredEntries : IEntries
{
    private readonly IEntries _entries;
    private readonly IEntriesFilter _filter;
    private readonly string _text;

    /// <summary>
    /// Creates filtering behavior for entries. Usage example: var entries = new FilteredEntries(inner, filter, message).
    /// </summary>
    /// <param name="entries">Source entries.</param>
    /// <param name="filter">Node filter.</param>
    /// <param name="text">Missing entries message.</param>
    public FilteredEntries(IEntries entries, IEntriesFilter filter, string text)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(filter);
        ArgumentException.ThrowIfNullOrEmpty(text);
        _entries = entries;
        _filter = filter;
        _text = text;
    }

    /// <summary>
    /// Returns filtered entries as structured content. Usage example: JsonNode node = entries.StructuredContent().
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
        if (list.Count == 0)
        {
            throw new InvalidOperationException(_text);
        }
        return list;
    }

}
