using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for a single order entry. Usage example: JsonNode node = new OrderSchema().Node(item).
/// </summary>
internal sealed class OrderSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an order schema with fields. Usage example: var schema = new OrderSchema().
    /// </summary>
    public OrderSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("NumEDocument"),
            new WholeRule("ClientOrderNum"),
            new WholeRule("IdAccount"),
            new WholeRule("IdSubAccount"),
            new WholeRule("IdRazdel"),
            new WholeRule("IdAllowedOrderParams"),
            new TextRule("AcceptTime"),
            new WholeRule("IdOrderType"),
            new WholeRule("IdObject"),
            new WholeRule("IdMarketBoard"),
            new RealRule("LimitPrice"),
            new WholeRule("BuySell"),
            new WholeRule("Quantity"),
            new TextRule("Comment"),
            new TextRule("Login"),
            new WholeRule("IdOrderStatus"),
            new WholeRule("Rest"),
            new RealRule("Price"),
            new TextRule("BrokerComment")
        ]);
    }

    /// <summary>
    /// Returns an output node for the order element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
