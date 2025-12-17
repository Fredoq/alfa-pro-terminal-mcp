using System.Text.Json;
using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Adds an array property built from a nested schema and maps missing arrays to an empty array. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class ArrayRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;
    private readonly IJsonSchema _schema;

    /// <summary>
    /// Creates an array rule using the same name for output and input property. Usage example: new ArrayRule("Instruments", schema).
    /// </summary>
    public ArrayRule(string name, IJsonSchema schema) : this(name, name, schema)
    {
    }

    /// <summary>
    /// Creates an array rule with separate output and input property names. Usage example: new ArrayRule("Items", "Data", schema).
    /// </summary>
    public ArrayRule(string name, string prop, IJsonSchema schema)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _prop = prop ?? throw new ArgumentNullException(nameof(prop));
        _schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    /// <summary>
    /// Applies the rule by building an array with nested schema nodes. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
    {
        JsonArray list = [];
        if (node.TryGetProperty(_prop, out JsonElement data) && data.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in data.EnumerateArray())
            {
                list.Add(_schema.Node(item));
            }
        }
        root[_name] = list;
    }
}

