using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for an order entry value element. Usage example: JsonNode node = new OrderEntryValueSchema().Node(item).
/// </summary>
internal sealed class OrderEntryValueSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an order entry value schema with fields. Usage example: var schema = new OrderEntryValueSchema().
    /// </summary>
    public OrderEntryValueSchema()
    {
        _schema = new RulesSchema([new WholeRule("ClientOrderNum"), new WholeRule("NumEDocument"), new WholeRule("ErrorCode"), new TextRule("ErrorText")]);
    }

    /// <summary>
    /// Returns an output node for the value element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
