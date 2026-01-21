using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Returns a JSON object stored in a payload. Usage example: JsonNode node = new PayloadObjectEntries(payload).StructuredContent().
/// </summary>
internal sealed class PayloadObjectEntries : IEntries
{
    private readonly string _payload;
    private readonly string _name;

    /// <summary>
    /// Creates entries for a payload object. Usage example: var entries = new PayloadObjectEntries(payload).
    /// </summary>
    /// <param name="payload">Router payload.</param>
    public PayloadObjectEntries(string payload) : this(payload, string.Empty)
    {
    }

    /// <summary>
    /// Creates entries for a named object field. Usage example: var entries = new PayloadObjectEntries(payload, "Value").
    /// </summary>
    /// <param name="payload">Router payload.</param>
    /// <param name="name">Object field name.</param>
    public PayloadObjectEntries(string payload, string name)
    {
        _payload = payload;
        _name = name;
    }

    /// <summary>
    /// Returns the object payload as structured content. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent()
    {
        JsonNode node = JsonNode.Parse(_payload) ?? throw new MissingEntriesException("Response payload object is missing");
        JsonObject root;
        try
        {
            root = node.AsObject();
        }
        catch (InvalidOperationException)
        {
            throw new MissingEntriesException("Response payload object is missing");
        }
        if (_name.Length == 0)
        {
            return root;
        }
        if (!root.TryGetPropertyValue(_name, out JsonNode? value) || value is null)
        {
            throw new MissingEntriesException("Response payload object is missing");
        }
        try
        {
            return value.AsObject();
        }
        catch (InvalidOperationException)
        {
            throw new MissingEntriesException("Response payload object is missing");
        }
    }
}
