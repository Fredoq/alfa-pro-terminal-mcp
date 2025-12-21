using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a described text field to an output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class TextRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;
    private readonly string _text;

    /// <summary>
    /// Creates a text rule using the same name for output and input property. Usage example: new TextRule("Ticker", description).
    /// </summary>
    public TextRule(string name, string text) : this(name, name, text)
    {
    }

    /// <summary>
    /// Creates a text rule with separate output and input property names. Usage example: new TextRule("Time", "DT", description).
    /// </summary>
    public TextRule(string name, string prop, string text)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _prop = prop ?? throw new ArgumentNullException(nameof(prop));
        _text = text ?? throw new ArgumentNullException(nameof(text));
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
    {
        string value = new JsonString(node, _prop).Value();
        JsonObject field = new()
        {
            ["value"] = JsonValue.Create(value),
            ["description"] = _text
        };
        root[_name] = field;
    }
}

