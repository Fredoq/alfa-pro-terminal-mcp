using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Returns a JSON array stored in a payload field. Usage example: string json = new PayloadArrayEntries(payload, "Data").Json().
/// </summary>
internal sealed class PayloadArrayEntries : IEntries
{
    private readonly string _payload;
    private readonly string _name;

    /// <summary>
    /// Creates entries for the default Data array field. Usage example: var entries = new PayloadArrayEntries(payload).
    /// </summary>
    /// <param name="payload">Router payload.</param>
    public PayloadArrayEntries(string payload) : this(payload, "Data")
    {
    }

    /// <summary>
    /// Creates entries for the specified array field. Usage example: var entries = new PayloadArrayEntries(payload, "OHLCV").
    /// </summary>
    /// <param name="payload">Router payload.</param>
    /// <param name="name">Array field name.</param>
    public PayloadArrayEntries(string payload, string name)
    {
        _payload = payload;
        _name = name;
    }

    /// <summary>
    /// Returns the array from the payload field as JSON. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        if (string.IsNullOrEmpty(_payload))
        {
            throw new InvalidOperationException("Payload is empty");
        }
        if (string.IsNullOrEmpty(_name))
        {
            throw new InvalidOperationException("Payload field name is empty");
        }
        using JsonDocument document = JsonDocument.Parse(_payload);
        JsonElement root = document.RootElement;
        if (!root.TryGetProperty(_name, out JsonElement data) || data.ValueKind != JsonValueKind.Array)
        {
            throw new MissingEntriesException("Response data array is missing");
        }
        return data.GetRawText();
    }
}
