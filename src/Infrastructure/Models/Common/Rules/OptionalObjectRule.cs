using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds an object property using a fallback when the source object is missing or null. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class OptionalObjectRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;
    private readonly IJsonSchema _schema;
    private readonly JsonObject _seed;

    /// <summary>
    /// Creates an optional object rule using the same name for output and input property. Usage example: new OptionalObjectRule("Error", schema, seed).
    /// </summary>
    public OptionalObjectRule(string name, IJsonSchema schema, JsonObject seed) : this(name, name, schema, seed)
    {
    }

    /// <summary>
    /// Creates an optional object rule with separate output and input property names. Usage example: new OptionalObjectRule("Error", "Error", schema, seed).
    /// </summary>
    public OptionalObjectRule(string name, string prop, IJsonSchema schema, JsonObject seed)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(prop);
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(seed);
        _name = name;
        _prop = prop;
        _schema = schema;
        _seed = seed;
    }

    /// <summary>
    /// Applies the rule by building an object with nested schema nodes. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonObject node, JsonObject root)
    {
        JsonObject item = new();
        foreach (KeyValuePair<string, JsonNode?> part in _seed)
        {
            item[part.Key] = part.Value?.DeepClone();
        }
        if (node.TryGetPropertyValue(_prop, out JsonNode? data) && data is not null)
        {
            if (data is JsonValue value)
            {
                if (value.ToJsonString() != "null")
                {
                    throw new InvalidOperationException("Entry object is missing");
                }
                root[_name] = _schema.Node(item);
                return;
            }
            JsonObject source;
            try
            {
                source = data.AsObject();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Entry object is missing");
            }
            foreach (KeyValuePair<string, JsonNode?> part in source)
            {
                if (part.Value is null)
                {
                    continue;
                }
                if (part.Value is JsonValue current && current.ToJsonString() == "null")
                {
                    continue;
                }
                item[part.Key] = part.Value.DeepClone();
            }
        }
        root[_name] = _schema.Node(item);
    }
}
