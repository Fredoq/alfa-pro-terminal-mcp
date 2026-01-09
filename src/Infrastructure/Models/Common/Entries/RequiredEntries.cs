using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Ensures that entries contain at least one item. Usage example: JsonNode node = new RequiredEntries(entries, "Entries are missing").StructuredContent().
/// </summary>
internal sealed class RequiredEntries : IEntries
{
    private readonly IEntries _entries;
    private readonly string _text;

    /// <summary>
    /// Creates required entries behavior. Usage example: var entries = new RequiredEntries(inner, message).
    /// </summary>
    /// <param name="entries">Source entries.</param>
    /// <param name="text">Missing entries message.</param>
    public RequiredEntries(IEntries entries, string text)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentException.ThrowIfNullOrEmpty(text);
        _entries = entries;
        _text = text;
    }

    /// <summary>
    /// Returns entries when at least one item exists. Usage example: JsonNode node = entries.StructuredContent().
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
        if (array.Count == 0)
        {
            throw new InvalidOperationException(_text);
        }
        return node;
    }

}
