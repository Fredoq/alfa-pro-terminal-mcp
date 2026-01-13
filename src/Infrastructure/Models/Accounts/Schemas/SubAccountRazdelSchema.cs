using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for subaccount portfolio entries. Usage example: JsonNode node = new SubAccountRazdelSchema().Node(item).
/// </summary>
internal sealed class SubAccountRazdelSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a subaccount portfolio schema with fields. Usage example: var schema = new SubAccountRazdelSchema().
    /// </summary>
    public SubAccountRazdelSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdRazdel"),
            new WholeRule("IdAccount"),
            new WholeRule("IdSubAccount"),
            new WholeRule("IdRazdelGroup"),
            new TextRule("RCode")
        ]);
    }

    /// <summary>
    /// Returns an output node for the subaccount portfolio element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
