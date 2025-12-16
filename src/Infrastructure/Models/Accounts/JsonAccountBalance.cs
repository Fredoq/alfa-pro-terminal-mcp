using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds account balance JSON with field descriptions. Usage example: string json = new JsonAccountBalance(payload, accountId).Json().
/// </summary>
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
            long account = new JsonInteger(node, "IdAccount").Value();
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
        entry["DataId"] = Field(new JsonInteger(node, "DataId").Value(), _descriptions["DataId"]);
        entry["IdAccount"] = Field(new JsonInteger(node, "IdAccount").Value(), _descriptions["IdAccount"]);
        entry["IdSubAccount"] = Field(new JsonInteger(node, "IdSubAccount").Value(), _descriptions["IdSubAccount"]);
        entry["IdRazdelGroup"] = Field(new JsonInteger(node, "IdRazdelGroup").Value(), _descriptions["IdRazdelGroup"]);
        entry["MarginInitial"] = Field(new JsonDouble(node, "MarginInitial").Value(), _descriptions["MarginInitial"]);
        entry["MarginMinimum"] = Field(new JsonDouble(node, "MarginMinimum").Value(), _descriptions["MarginMinimum"]);
        entry["MarginRequirement"] = Field(new JsonDouble(node, "MarginRequirement").Value(), _descriptions["MarginRequirement"]);
        entry["Money"] = Field(new JsonDouble(node, "Money").Value(), _descriptions["Money"]);
        entry["MoneyInitial"] = Field(new JsonDouble(node, "MoneyInitial").Value(), _descriptions["MoneyInitial"]);
        entry["Balance"] = Field(new JsonDouble(node, "Balance").Value(), _descriptions["Balance"]);
        entry["PrevBalance"] = Field(new JsonDouble(node, "PrevBalance").Value(), _descriptions["PrevBalance"]);
        entry["PortfolioCost"] = Field(new JsonDouble(node, "PortfolioCost").Value(), _descriptions["PortfolioCost"]);
        entry["LiquidBalance"] = Field(new JsonDouble(node, "LiquidBalance").Value(), _descriptions["LiquidBalance"]);
        entry["Requirements"] = Field(new JsonDouble(node, "Requirements").Value(), _descriptions["Requirements"]);
        entry["ImmediateRequirements"] = Field(new JsonDouble(node, "ImmediateRequirements").Value(), _descriptions["ImmediateRequirements"]);
        entry["NPL"] = Field(new JsonDouble(node, "NPL").Value(), _descriptions["NPL"]);
        entry["DailyPL"] = Field(new JsonDouble(node, "DailyPL").Value(), _descriptions["DailyPL"]);
        entry["NPLPercent"] = Field(new JsonDouble(node, "NPLPercent").Value(), _descriptions["NPLPercent"]);
        entry["DailyPLPercent"] = Field(new JsonDouble(node, "DailyPLPercent").Value(), _descriptions["DailyPLPercent"]);
        entry["NKD"] = Field(new JsonDouble(node, "NKD").Value(), _descriptions["NKD"]);
        return entry;
    }

    private static JsonObject Field<T>(T value, string description) where T : notnull
    {
        JsonObject field = new();
        field["value"] = JsonValue.Create(value);
        field["description"] = description;
        return field;
    }
}
