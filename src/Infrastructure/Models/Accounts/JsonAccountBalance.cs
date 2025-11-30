using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds account balance JSON with field descriptions. Usage example: string json = new JsonAccountBalance(payload, accountId).Json().
/// </summary>
/// TODO: Refactor static methods
internal sealed class JsonAccountBalance : IAccountBalance
{
    private readonly string _payload;
    private readonly long _accountId;
    private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>
    {
        ["DataId"] = "Balance identifier computed as IdSubAccount * 8 + IdRazdelGroup",
        ["IdAccount"] = "Client account id",
        ["IdSubAccount"] = "Client subaccount id",
        ["IdRazdelGroup"] = "Portfolio group code",
        ["MarginInitial"] = "Initial margin",
        ["MarginMinimum"] = "Minimum margin",
        ["MarginRequirement"] = "Margin requirements",
        ["Money"] = "Cash in rubles",
        ["MoneyInitial"] = "Opening cash in rubles",
        ["Balance"] = "Balance value",
        ["PrevBalance"] = "Opening balance",
        ["PortfolioCost"] = "Portfolio value",
        ["LiquidBalance"] = "Liquid portfolio value",
        ["Requirements"] = "Requirements",
        ["ImmediateRequirements"] = "Immediate requirements",
        ["NPL"] = "Nominal profit or loss",
        ["DailyPL"] = "Daily profit or loss",
        ["NPLPercent"] = "Nominal PnL percent",
        ["DailyPLPercent"] = "Daily PnL percent",
        ["NKD"] = "Accrued coupon income"
    };

    /// <summary>
    /// Creates parsing behavior for balance payload. Usage example: var balance = new JsonAccountBalance(payload, accountId).
    /// </summary>
    /// <param name="payload">Router payload.</param>
    /// <param name="accountId">Target account identifier.</param>
    public JsonAccountBalance(string payload, long accountId)
    {
        _payload = payload;
        _accountId = accountId;
    }

    /// <summary>
    /// Returns account balance JSON enriched with field descriptions. Usage example: string json = balance.Json().
    /// </summary>
    public string Json()
    {
        using JsonDocument document = JsonDocument.Parse(_payload);
        JsonElement root = document.RootElement;
        if (!root.TryGetProperty("Data", out JsonElement data))
        {
            throw new InvalidOperationException("Response data array is missing");
        }
        if (data.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Response data array is missing");
        }
        JsonArray list = [];
        foreach (JsonElement node in data.EnumerateArray())
        {
            long account = Integer(node, "IdAccount");
            if (account != _accountId)
            {
                continue;
            }
            list.Add(Entry(node));
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Account balance is missing");
        }
        return JsonSerializer.Serialize(list);
    }

    private static JsonObject Entry(JsonElement node)
    {
        JsonObject entry = new();
        entry["DataId"] = Field(Integer(node, "DataId"), _descriptions["DataId"]);
        entry["IdAccount"] = Field(Integer(node, "IdAccount"), _descriptions["IdAccount"]);
        entry["IdSubAccount"] = Field(Integer(node, "IdSubAccount"), _descriptions["IdSubAccount"]);
        entry["IdRazdelGroup"] = Field(Integer(node, "IdRazdelGroup"), _descriptions["IdRazdelGroup"]);
        entry["MarginInitial"] = Field(Double(node, "MarginInitial"), _descriptions["MarginInitial"]);
        entry["MarginMinimum"] = Field(Double(node, "MarginMinimum"), _descriptions["MarginMinimum"]);
        entry["MarginRequirement"] = Field(Double(node, "MarginRequirement"), _descriptions["MarginRequirement"]);
        entry["Money"] = Field(Double(node, "Money"), _descriptions["Money"]);
        entry["MoneyInitial"] = Field(Double(node, "MoneyInitial"), _descriptions["MoneyInitial"]);
        entry["Balance"] = Field(Double(node, "Balance"), _descriptions["Balance"]);
        entry["PrevBalance"] = Field(Double(node, "PrevBalance"), _descriptions["PrevBalance"]);
        entry["PortfolioCost"] = Field(Double(node, "PortfolioCost"), _descriptions["PortfolioCost"]);
        entry["LiquidBalance"] = Field(Double(node, "LiquidBalance"), _descriptions["LiquidBalance"]);
        entry["Requirements"] = Field(Double(node, "Requirements"), _descriptions["Requirements"]);
        entry["ImmediateRequirements"] = Field(Double(node, "ImmediateRequirements"), _descriptions["ImmediateRequirements"]);
        entry["NPL"] = Field(Double(node, "NPL"), _descriptions["NPL"]);
        entry["DailyPL"] = Field(Double(node, "DailyPL"), _descriptions["DailyPL"]);
        entry["NPLPercent"] = Field(Double(node, "NPLPercent"), _descriptions["NPLPercent"]);
        entry["DailyPLPercent"] = Field(Double(node, "DailyPLPercent"), _descriptions["DailyPLPercent"]);
        entry["NKD"] = Field(Double(node, "NKD"), _descriptions["NKD"]);
        return entry;
    }

    private static JsonObject Field<T>(T value, string description) where T : notnull
    {
        JsonObject field = new();
        field["value"] = JsonValue.Create(value);
        field["description"] = description;
        return field;
    }

    private static long Integer(JsonElement node, string property)
    {
        if (!node.TryGetProperty(property, out JsonElement value))
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        if (value.ValueKind != JsonValueKind.Number)
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        return value.GetInt64();
    }

    private static double Double(JsonElement node, string property)
    {
        if (!node.TryGetProperty(property, out JsonElement value))
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        if (value.ValueKind != JsonValueKind.Number)
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        return value.GetDouble();
    }
}
