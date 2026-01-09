using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

/// <summary>
/// Defines a JSON transformation rule applied to an output object. Usage example: rule.Apply(item, output).
/// </summary>
internal interface IJsonRule
{
    /// <summary>
    /// Applies the rule by reading the source JSON object and mutating the output object. Usage example: rule.Apply(item, output).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    /// <param name="root">Target output object.</param>
    void Apply(JsonObject node, JsonObject root);
}
