using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Returns a JSON array stored in a payload field. Usage example: JsonNode node = new PayloadArrayEntries(payload, "Data").StructuredContent().
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
        ArgumentException.ThrowIfNullOrEmpty(payload);
        ArgumentException.ThrowIfNullOrEmpty(name);
        _payload = payload;
        _name = name;
    }

    /// <summary>
    /// Returns the array from the payload field as structured content. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent()
    {
        JsonNode node = JsonNode.Parse(_payload) ?? throw new MissingEntriesException("Response data array is missing");
        JsonObject root = node.AsObject();
        if (!root.TryGetPropertyValue(_name, out JsonNode? data) || data is null)
        {
            throw new MissingEntriesException("Response data array is missing");
        }
        try
        {
            return data.AsArray();
        }
        catch (InvalidOperationException)
        {
            throw new MissingEntriesException("Response data array is missing");
        }
    }

}
