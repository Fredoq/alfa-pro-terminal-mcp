using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

/// <summary>
/// Composes an output JSON object by applying a set of rules. Usage example: JsonNode node = new RulesSchema(rules).Node(element).
/// </summary>
internal sealed class RulesSchema : IJsonSchema
{
    private readonly IReadOnlyList<IJsonRule> _rules;

    /// <summary>
    /// Creates a rule-based schema. Usage example: var schema = new RulesSchema([rule]).
    /// </summary>
    public RulesSchema(IReadOnlyList<IJsonRule> rules)
    {
        _rules = rules ?? throw new ArgumentNullException(nameof(rules));
    }

    /// <summary>
    /// Returns an output object for the source element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node)
    {
        JsonObject root = [];
        foreach (IJsonRule rule in _rules)
        {
            rule.Apply(node, root);
        }
        return root;
    }
}
