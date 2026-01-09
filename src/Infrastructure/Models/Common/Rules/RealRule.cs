using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Adds a real-number field to an output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class RealRule : IJsonRule
{
    private readonly string _name;
    private readonly string _prop;

    /// <summary>
    /// Creates a real-number rule using the same name for output and input property. Usage example: new RealRule("Price").
    /// </summary>
    public RealRule(string name) : this(name, name)
    {
    }

    /// <summary>
    /// Creates a real-number rule with separate output and input property names. Usage example: new RealRule("Time", "DT").
    /// </summary>
    public RealRule(string name, string prop)
    {
        _name = name;
        _prop = prop;
    }

    /// <summary>
    /// Applies the rule to the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonObject node, JsonObject root)
    {
        double value = new JsonDouble(node, _prop).Value();
        root[_name] = JsonValue.Create(value);
    }
}
