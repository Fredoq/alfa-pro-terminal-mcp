using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds an object property built from a nested schema. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class ObjectRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;
    private readonly IJsonSchema _schema;

    /// <summary>
    /// Creates an object rule using the same name for output and input property. Usage example: new ObjectRule("Error", schema).
    /// </summary>
    public ObjectRule(string name, IJsonSchema schema) : this(name, name, schema)
    {
    }

    /// <summary>
    /// Creates an object rule with separate output and input property names. Usage example: new ObjectRule("Error", "Error", schema).
    /// </summary>
    public ObjectRule(string name, string prop, IJsonSchema schema)
    {
        _name = name;
        _prop = prop;
        _schema = schema;
    }

    /// <summary>
    /// Applies the rule by building an object with nested schema nodes. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonObject node, JsonObject root)
    {
        if (!node.TryGetPropertyValue(_prop, out JsonNode? data) || data is null)
        {
            throw new InvalidOperationException("Entry object is missing");
        }
        JsonObject item;
        try
        {
            item = data.AsObject();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("Entry object is missing");
        }
        root[_name] = _schema.Node(item);
    }
}
