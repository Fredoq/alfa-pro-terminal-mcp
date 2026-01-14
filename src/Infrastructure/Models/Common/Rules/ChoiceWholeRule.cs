using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a whole-number field using a primary property with a fallback. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class ChoiceWholeRule : IJsonRule
{
    private readonly string _name;
    private readonly string _primary;
    private readonly string _secondary;

    /// <summary>
    /// Creates a whole-number rule with primary and fallback properties. Usage example: new ChoiceWholeRule("ResponseStatus", "ResponseStatus", "Status").
    /// </summary>
    public ChoiceWholeRule(string name, string primary, string secondary)
    {
        _name = name;
        _primary = primary;
        _secondary = secondary;
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonObject node, JsonObject root)
    {
        long value;
        try
        {
            value = new JsonInteger(node, _primary).Value();
        }
        catch (InvalidOperationException)
        {
            value = new JsonInteger(node, _secondary).Value();
        }
        root[_name] = JsonValue.Create(value);
    }
}
