using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a boolean field to an output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class FlagRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;

    /// <summary>
    /// Creates a boolean rule using the same name for output and input property. Usage example: new FlagRule("IsLiquid").
    /// </summary>
    public FlagRule(string name) : this(name, name)
    {
    }

    /// <summary>
    /// Creates a boolean rule with separate output and input property names. Usage example: new FlagRule("Flag", "IsFlag").
    /// </summary>
    public FlagRule(string name, string prop)
    {
        _name = name;
        _prop = prop;
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
    {
        bool value = new JsonBool(node, _prop).Value();
        root[_name] = JsonValue.Create(value);
    }
}
