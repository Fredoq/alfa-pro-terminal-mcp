using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Defines an output schema for a single account balance entry. Usage example: JsonNode node = new AccountBalanceSchema().Node(element).
/// </summary>
internal sealed class AccountBalanceSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an account balance schema with described fields. Usage example: var schema = new AccountBalanceSchema().
    /// </summary>
    public AccountBalanceSchema()
    {
        AccountBalanceDescriptions text = new();
        JsonIntegerItem integer = new();
        JsonDoubleItem doubley = new();
        _schema = new RulesSchema([
            new ValueRule<long>(integer, "DataId", text.Text("DataId")),
            new ValueRule<long>(integer, "IdAccount", text.Text("IdAccount")),
            new ValueRule<long>(integer, "IdSubAccount", text.Text("IdSubAccount")),
            new ValueRule<long>(integer, "IdRazdelGroup", text.Text("IdRazdelGroup")),
            new ValueRule<double>(doubley, "MarginInitial", text.Text("MarginInitial")),
            new ValueRule<double>(doubley, "MarginMinimum", text.Text("MarginMinimum")),
            new ValueRule<double>(doubley, "MarginRequirement", text.Text("MarginRequirement")),
            new ValueRule<double>(doubley, "Money", text.Text("Money")),
            new ValueRule<double>(doubley, "MoneyInitial", text.Text("MoneyInitial")),
            new ValueRule<double>(doubley, "Balance", text.Text("Balance")),
            new ValueRule<double>(doubley, "PrevBalance", text.Text("PrevBalance")),
            new ValueRule<double>(doubley, "PortfolioCost", text.Text("PortfolioCost")),
            new ValueRule<double>(doubley, "LiquidBalance", text.Text("LiquidBalance")),
            new ValueRule<double>(doubley, "Requirements", text.Text("Requirements")),
            new ValueRule<double>(doubley, "ImmediateRequirements", text.Text("ImmediateRequirements")),
            new ValueRule<double>(doubley, "NPL", text.Text("NPL")),
            new ValueRule<double>(doubley, "DailyPL", text.Text("DailyPL")),
            new ValueRule<double>(doubley, "NPLPercent", text.Text("NPLPercent")),
            new ValueRule<double>(doubley, "DailyPLPercent", text.Text("DailyPLPercent")),
            new ValueRule<double>(doubley, "NKD", text.Text("NKD"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the balance element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
