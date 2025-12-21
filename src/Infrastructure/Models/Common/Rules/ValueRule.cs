using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a described scalar property to an output object. Usage example: rule.Apply(element, output).
/// </summary>
/// <typeparam name="T">Scalar value type.</typeparam>
internal sealed class ValueRule<T> : IJsonRule where T : notnull
{
    private readonly IJsonValue<T> _value;
    private readonly string _name;
    private readonly string _text;

    /// <summary>
    /// Creates a described rule. Usage example: new ValueRule(new JsonInteger(node, "Id"), "Id", description).
    /// </summary>
    public ValueRule(IJsonValue<T> value, string name, string text)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _text = text ?? throw new ArgumentNullException(nameof(text));
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
    {
        T value = _value.Value();
        JsonObject field = new()
        {
            ["value"] = JsonValue.Create(value),
            ["description"] = _text
        };
        root[_name] = field;
    }
}
