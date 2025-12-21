using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Describes positions JSON using a schema. Usage example: string json = new DescribedPositionsEntries(entries, schema).Json().
/// </summary>
internal sealed class DescribedPositionsEntries : IPositionsEntries
{
    private readonly IPositionsEntries _entries;
    private readonly IJsonSchema _schema;

    /// <summary>
    /// Creates schema description behavior over positions JSON. Usage example: var entries = new DescribedPositionsEntries(inner, schema).
    /// </summary>
    /// <param name="entries">Source positions entries.</param>
    /// <param name="schema">Position schema.</param>
    public DescribedPositionsEntries(IPositionsEntries entries, IJsonSchema schema)
    {
        _entries = entries ?? throw new ArgumentNullException(nameof(entries));
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    /// <summary>
    /// Returns described positions JSON. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        string json = _entries.Json();
        JsonDocument document = JsonDocument.Parse(json);
        using (document)
        {
            JsonElement root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Positions array is missing");
            }
            JsonArray list = [];
            foreach (JsonElement item in root.EnumerateArray())
            {
                list.Add(_schema.Node(item));
            }
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Positions are missing");
            }
            return JsonSerializer.Serialize(list);
        }
    }
}
