using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for an order entry error element. Usage example: JsonNode node = new OrderEntryErrorSchema().Node(item).
/// </summary>
internal sealed class OrderEntryErrorSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an order entry error schema with fields. Usage example: var schema = new OrderEntryErrorSchema().
    /// </summary>
    public OrderEntryErrorSchema()
    {
        _schema = new RulesSchema([new WholeRule("Code"), new TextRule("Message")]);
    }

    /// <summary>
    /// Returns an output node for the error element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
