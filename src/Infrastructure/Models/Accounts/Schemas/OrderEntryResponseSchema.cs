using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for an order entry response. Usage example: JsonNode node = new OrderEntryResponseSchema().Node(item).
/// </summary>
internal sealed class OrderEntryResponseSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an order entry response schema with fields. Usage example: var schema = new OrderEntryResponseSchema().
    /// </summary>
    public OrderEntryResponseSchema()
    {
        JsonObject seed = new() { ["Code"] = 0L, ["Message"] = string.Empty };
        _schema = new RulesSchema([new ChoiceWholeRule("ResponseStatus", "ResponseStatus", "Status"), new TextRule("Message"), new OptionalObjectRule("Error", new OrderEntryErrorSchema(), seed), new ObjectRule("Value", new OrderEntryValueSchema())]);
    }

    /// <summary>
    /// Returns an output node for the response element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
