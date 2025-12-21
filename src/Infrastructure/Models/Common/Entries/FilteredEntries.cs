using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Filters JSON array entries by a predicate. Usage example: string json = new FilteredEntries(entries, filter, "Entries are missing").Json().
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
    /// Returns filtered entries as JSON. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        string json = _entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Entries array is missing");
        }
        JsonArray list = [];
        foreach (JsonElement item in root.EnumerateArray())
        {
            if (!_filter.Filtered(item))
            {
                continue;
            }
            JsonNode node = JsonNode.Parse(item.GetRawText()) ?? throw new InvalidOperationException("Entry node is missing");
            list.Add(node);
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException(_text);
        }
        return JsonSerializer.Serialize(list);
    }
}
