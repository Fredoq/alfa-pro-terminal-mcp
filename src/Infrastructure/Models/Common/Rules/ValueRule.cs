using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a scalar property to an output object. Usage example: rule.Apply(element, output).
/// </summary>
/// <typeparam name="T">Scalar value type.</typeparam>
internal sealed class ValueRule<T> : IJsonRule where T : notnull
{
    private readonly IJsonValue<T> _value;
    private readonly string _name;

    /// <summary>
    /// Creates a scalar rule. Usage example: new ValueRule(new JsonInteger(node, "Id"), "Id").
    /// </summary>
    public ValueRule(IJsonValue<T> value, string name)
    {
        _value = value;
        _name = name;
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
    {
        T value = _value.Value();
        root[_name] = JsonValue.Create(value);
    }
}
