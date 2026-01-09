using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Wraps entries into a root object property. Usage example: JsonNode node = new RootEntries(entries, "accounts").StructuredContent().
/// </summary>
public sealed class RootEntries : IEntries
{
    private readonly IEntries _entries;
    private readonly string _name;

    /// <summary>
    /// Creates root wrapping for entries. Usage example: var entries = new RootEntries(inner, "accounts").
    /// </summary>
    /// <param name="entries">Source entries.</param>
    /// <param name="name">Root property name.</param>
    public RootEntries(IEntries entries, string name)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentException.ThrowIfNullOrEmpty(name);
        _entries = entries;
        _name = name;
    }

    /// <summary>
    /// Returns entries wrapped into a root property. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent() => new JsonObject() { [_name] = _entries.StructuredContent() };

}
