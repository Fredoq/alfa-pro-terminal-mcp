using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Maps JSON array entries through a schema. Usage example: string json = new SchemaEntries(entries, schema).Json().
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
        _entries = entries;
        _schema = schema;
    }

    /// <summary>
    /// Returns mapped entries as JSON. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        if (_entries is null || _schema is null)
        {
            throw new InvalidOperationException("Entries configuration is missing");
        }
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
            list.Add(_schema.Node(item));
        }
        return JsonSerializer.Serialize(list);
    }
}
