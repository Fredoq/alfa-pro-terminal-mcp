using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a described real-number field to an output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class RealRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;
    private readonly string _text;

    /// <summary>
    /// Creates a real-number rule using the same name for output and input property. Usage example: new RealRule("Price", description).
    /// </summary>
    public RealRule(string name, string text) : this(name, name, text)
    {
    }

    /// <summary>
    /// Creates a real-number rule with separate output and input property names. Usage example: new RealRule("Time", "DT", description).
    /// </summary>
    public RealRule(string name, string prop, string text)
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
        double value = new JsonDouble(node, _prop).Value();
        JsonObject field = new()
        {
            ["value"] = JsonValue.Create(value),
            ["description"] = _text
        };
        root[_name] = field;
    }
}

