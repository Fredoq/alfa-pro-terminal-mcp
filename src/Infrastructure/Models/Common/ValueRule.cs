using System.Text.Json;
using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Adds a described scalar property to an output object. Usage example: rule.Apply(element, output).
/// </summary>
/// <typeparam name="T">Scalar value type.</typeparam>
internal sealed class ValueRule<T> : IJsonRule where T : notnull
{
    private readonly IJsonItem<T> _item;
    private readonly string _name;
    private readonly string _prop;
    private readonly string _text;

    /// <summary>
    /// Creates a described rule using the same name for output and input property. Usage example: new ValueRule(item, "Id", description).
    /// </summary>
    public ValueRule(IJsonItem<T> item, string name, string text) : this(item, name, name, text)
    {
    }

    /// <summary>
    /// Creates a described rule with separate output and input property names. Usage example: new ValueRule(item, "Time", "DT", description).
    /// </summary>
    public ValueRule(IJsonItem<T> item, string name, string prop, string text)
    {
        _item = item ?? throw new ArgumentNullException(nameof(item));
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _prop = prop ?? throw new ArgumentNullException(nameof(prop));
        _text = text ?? throw new ArgumentNullException(nameof(text));
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
    {
        T value = _item.Value(node, _prop).Value();
        JsonObject field = new()
        {
            ["value"] = JsonValue.Create(value),
            ["description"] = _text
        };
        root[_name] = field;
    }
}

