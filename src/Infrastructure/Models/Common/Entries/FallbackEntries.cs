using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Chooses the first available entries from two sources. Usage example: JsonNode node = new FallbackEntries(primary, secondary).StructuredContent().
/// </summary>
internal sealed class FallbackEntries : IEntries
{
    private readonly IEntries _first;
    private readonly IEntries _second;

    /// <summary>
    /// Creates fallback behavior for entries. Usage example: var entries = new FallbackEntries(primary, secondary).
    /// </summary>
    /// <param name="first">Primary entries.</param>
    /// <param name="second">Secondary entries.</param>
    public FallbackEntries(IEntries first, IEntries second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        _first = first;
        _second = second;
    }

    /// <summary>
    /// Returns entries from the first source or falls back to the second. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent()
    {
        try
        {
            return _first.StructuredContent();
        }
        catch (MissingEntriesException)
        {
            return _second.StructuredContent();
        }
    }

}
