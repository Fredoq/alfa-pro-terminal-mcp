using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a text field to an output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class TextRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;

    /// <summary>
    /// Creates a text rule using the same name for output and input property. Usage example: new TextRule("Ticker").
    /// </summary>
    public TextRule(string name) : this(name, name)
    {
    }

    /// <summary>
    /// Creates a text rule with separate output and input property names. Usage example: new TextRule("Time", "DT").
    /// </summary>
    public TextRule(string name, string prop)
    {
        _name = name;
        _prop = prop;
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonObject node, JsonObject root)
    {
        string value = new JsonString(node, _prop).Value();
        root[_name] = JsonValue.Create(value);
    }
}
