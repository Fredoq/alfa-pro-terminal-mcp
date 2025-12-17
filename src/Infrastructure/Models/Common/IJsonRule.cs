using System.Text.Json;
using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Defines a JSON transformation rule applied to an output object. Usage example: rule.Apply(element, output).
/// </summary>
internal interface IJsonRule
{
    /// <summary>
    /// Applies the rule by reading the source JSON element and mutating the output object. Usage example: rule.Apply(element, output).
    /// </summary>
    /// <param name="node">Source JSON element.</param>
    /// <param name="root">Target output object.</param>
    void Apply(JsonElement node, JsonObject root);
}

