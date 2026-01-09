using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Maps JSON array entries through a schema. Usage example: JsonNode node = new SchemaEntries(entries, schema).StructuredContent().
/// </summary>
internal sealed class SchemaEntries : IEntries
{
    private readonly IEntries _entries;
    private readonly IJsonSchema _schema;

    /// <summary>
    /// Creates schema mapping behavior for entries. Usage example: var entries = new SchemaEntries(inner, schema).
    /// </summary>
    /// <param name="entries">Source entries.</param>
    /// <param name="schema">Schema mapping.</param>
    public SchemaEntries(IEntries entries, IJsonSchema schema)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(schema);
        _entries = entries;
        _schema = schema;
    }

    /// <summary>
    /// Returns mapped entries as structured content. Usage example: JsonNode node = entries.StructuredContent().
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
            list.Add(_schema.Node(item.AsObject()));
        }
        return list;
    }

    /// <summary>
    /// Returns mapped entries as JSON text. Usage example: string json = entries.Text().
    /// </summary>
    public string Text() => StructuredContent().ToJsonString();
}
