using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Defines an output schema for a single account balance entry. Usage example: JsonNode node = new AccountBalanceSchema().Node(element).
/// </summary>
internal sealed class AccountBalanceSchema : IJsonSchema
{
    /// <summary>
    /// Returns an output node for the balance element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node)
    {
        AccountBalanceDescriptions text = new();
        RulesSchema schema = new([
            new ValueRule<long>(new JsonInteger(node, "DataId"), "DataId", text.Text("DataId")),
            new ValueRule<long>(new JsonInteger(node, "IdAccount"), "IdAccount", text.Text("IdAccount")),
            new ValueRule<long>(new JsonInteger(node, "IdSubAccount"), "IdSubAccount", text.Text("IdSubAccount")),
            new ValueRule<long>(new JsonInteger(node, "IdRazdelGroup"), "IdRazdelGroup", text.Text("IdRazdelGroup")),
            new ValueRule<double>(new JsonDouble(node, "MarginInitial"), "MarginInitial", text.Text("MarginInitial")),
            new ValueRule<double>(new JsonDouble(node, "MarginMinimum"), "MarginMinimum", text.Text("MarginMinimum")),
            new ValueRule<double>(new JsonDouble(node, "MarginRequirement"), "MarginRequirement", text.Text("MarginRequirement")),
            new ValueRule<double>(new JsonDouble(node, "Money"), "Money", text.Text("Money")),
            new ValueRule<double>(new JsonDouble(node, "MoneyInitial"), "MoneyInitial", text.Text("MoneyInitial")),
            new ValueRule<double>(new JsonDouble(node, "Balance"), "Balance", text.Text("Balance")),
            new ValueRule<double>(new JsonDouble(node, "PrevBalance"), "PrevBalance", text.Text("PrevBalance")),
            new ValueRule<double>(new JsonDouble(node, "PortfolioCost"), "PortfolioCost", text.Text("PortfolioCost")),
            new ValueRule<double>(new JsonDouble(node, "LiquidBalance"), "LiquidBalance", text.Text("LiquidBalance")),
            new ValueRule<double>(new JsonDouble(node, "Requirements"), "Requirements", text.Text("Requirements")),
            new ValueRule<double>(new JsonDouble(node, "ImmediateRequirements"), "ImmediateRequirements", text.Text("ImmediateRequirements")),
            new ValueRule<double>(new JsonDouble(node, "NPL"), "NPL", text.Text("NPL")),
            new ValueRule<double>(new JsonDouble(node, "DailyPL"), "DailyPL", text.Text("DailyPL")),
            new ValueRule<double>(new JsonDouble(node, "NPLPercent"), "NPLPercent", text.Text("NPLPercent")),
            new ValueRule<double>(new JsonDouble(node, "DailyPLPercent"), "DailyPLPercent", text.Text("DailyPLPercent")),
            new ValueRule<double>(new JsonDouble(node, "NKD"), "NKD", text.Text("NKD"))
        ]);
        return schema.Node(node);
    }
}
