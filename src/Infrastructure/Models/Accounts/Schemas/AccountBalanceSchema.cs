using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;
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
    /// Creates a balance schema with described fields. Usage example: var schema = new AccountBalanceSchema().
    /// </summary>
    public AccountBalanceSchema()
    {
        AccountBalanceDescriptions text = new();
        _schema = new RulesSchema([
            new WholeRule("DataId", text.Text("DataId")),
            new WholeRule("IdAccount", text.Text("IdAccount")),
            new WholeRule("IdSubAccount", text.Text("IdSubAccount")),
            new WholeRule("IdRazdelGroup", text.Text("IdRazdelGroup")),
            new RealRule("MarginInitial", text.Text("MarginInitial")),
            new RealRule("MarginMinimum", text.Text("MarginMinimum")),
            new RealRule("MarginRequirement", text.Text("MarginRequirement")),
            new RealRule("Money", text.Text("Money")),
            new RealRule("MoneyInitial", text.Text("MoneyInitial")),
            new RealRule("Balance", text.Text("Balance")),
            new RealRule("PrevBalance", text.Text("PrevBalance")),
            new RealRule("PortfolioCost", text.Text("PortfolioCost")),
            new RealRule("LiquidBalance", text.Text("LiquidBalance")),
            new RealRule("Requirements", text.Text("Requirements")),
            new RealRule("ImmediateRequirements", text.Text("ImmediateRequirements")),
            new RealRule("NPL", text.Text("NPL")),
            new RealRule("DailyPL", text.Text("DailyPL")),
            new RealRule("NPLPercent", text.Text("NPLPercent")),
            new RealRule("DailyPLPercent", text.Text("DailyPLPercent")),
            new RealRule("NKD", text.Text("NKD"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the balance element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
