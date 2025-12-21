using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Ensures that entries contain at least one item. Usage example: string json = new RequiredEntries(entries, "Entries are missing").Json().
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
    /// Returns entries when at least one item exists. Usage example: string json = entries.Json().
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
        if (root.GetArrayLength() == 0)
        {
            throw new InvalidOperationException(_text);
        }
        return json;
    }
}
