using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for market board entries. Usage example: JsonNode node = new MarketBoardSchema().Node(item).
/// </summary>
internal sealed class MarketBoardSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a market board schema with fields. Usage example: var schema = new MarketBoardSchema().
    /// <summary>
    /// Initializes the MarketBoardSchema with the rules for IdMarketBoard, NameMarketBoard, DescMarketBoard, RCode, and IdObjectCurrency.
    /// </summary>
    public MarketBoardSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdMarketBoard"),
            new TextRule("NameMarketBoard"),
            new TextRule("DescMarketBoard"),
            new TextRule("RCode"),
            new WholeRule("IdObjectCurrency")
        ]);
    }

    /// <summary>
    /// Returns an output node for the market board element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <summary>
/// Produce a JsonNode representing a market board entry from the provided JSON object.
/// </summary>
/// <param name="node">Source JSON object containing market board fields.</param>
/// <returns>A JsonNode containing the market board fields as defined by this schema.</returns>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}