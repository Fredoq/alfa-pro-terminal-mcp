using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Maps JSON object entry through a schema. Usage example: JsonNode node = new SchemaEntry(entries, schema).StructuredContent().
/// </summary>
internal sealed class SchemaEntry : IEntries
{
    private readonly IEntries _entries;
    private readonly IJsonSchema _schema;

    /// <summary>
    /// Creates schema mapping behavior for a single entry. Usage example: var entries = new SchemaEntry(inner, schema).
    /// </summary>
    /// <param name="entries">Source entries.</param>
    /// <param name="schema">Schema mapping.</param>
    public SchemaEntry(IEntries entries, IJsonSchema schema)
    {
        _entries = entries;
        _schema = schema;
    }

    /// <summary>
    /// Returns mapped entry as structured content. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent()
    {
        JsonNode node = _entries.StructuredContent();
        JsonObject root;
        try
        {
            root = node.AsObject();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("Entry object is missing");
        }
        return _schema.Node(root);
    }
}
