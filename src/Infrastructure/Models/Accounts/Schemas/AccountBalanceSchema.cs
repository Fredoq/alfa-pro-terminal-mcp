using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for a single account balance entry. Usage example: JsonNode node = new AccountBalanceSchema().Node(element).
/// </summary>
internal sealed class AccountBalanceSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a balance schema with fields. Usage example: var schema = new AccountBalanceSchema().
    /// </summary>
    public AccountBalanceSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("DataId"),
            new WholeRule("IdAccount"),
            new WholeRule("IdSubAccount"),
            new WholeRule("IdRazdelGroup"),
            new RealRule("MarginInitial"),
            new RealRule("MarginMinimum"),
            new RealRule("MarginRequirement"),
            new RealRule("Money"),
            new RealRule("MoneyInitial"),
            new RealRule("Balance"),
            new RealRule("PrevBalance"),
            new RealRule("PortfolioCost"),
            new RealRule("LiquidBalance"),
            new RealRule("Requirements"),
            new RealRule("ImmediateRequirements"),
            new RealRule("NPL"),
            new RealRule("DailyPL"),
            new RealRule("NPLPercent"),
            new RealRule("DailyPLPercent"),
            new RealRule("NKD")
        ]);
    }

    /// <summary>
    /// Returns an output node for the balance element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
