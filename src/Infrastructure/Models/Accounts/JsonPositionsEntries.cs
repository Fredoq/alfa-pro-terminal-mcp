using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds position entries from JSON payload. Usage example: string json = new JsonPositionsEntries(payload, accountId).Json().
/// </summary>
/// TODO: Refactor static methods
internal sealed class JsonPositionsEntries : IPositionsEntries
{
    private readonly string _payload;
    private readonly long _accountId;
    private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>
    {
        ["IdPosition"] = "Position identifier",
        ["IdAccount"] = "Client account id",
        ["IdSubAccount"] = "Client subaccount id",
        ["IdRazdel"] = "Portfolio id",
        ["IdObject"] = "Security identifier",
        ["IdFiBalance"] = "Financial instrument used for valuation",
        ["IdBalanceGroup"] = "Portfolio group identifier",
        ["AssetsPercent"] = "Position share in subaccount percent",
        ["PSTNKD"] = "Accrued coupon income",
        ["IsMoney"] = "Indicates money position",
        ["IsRur"] = "Indicates ruble currency position",
        ["UchPrice"] = "Accounting price",
        ["TorgPos"] = "Current position size",
        ["Price"] = "Current price",
        ["DailyPL"] = "Daily profit or loss",
        ["DailyPLPercentToMarketCurPrice"] = "Daily PnL percent to market price",
        ["BackPos"] = "Opening position",
        ["PrevQuote"] = "Previous session close price",
        ["TrnIn"] = "External credit volume",
        ["TrnOut"] = "External debit volume",
        ["DailyBuyVolume"] = "Session buy volume",
        ["DailySellVolume"] = "Session sell volume",
        ["DailyBuyQuantity"] = "Session buy quantity",
        ["DailySellQuantity"] = "Session sell quantity",
        ["NKD"] = "Accrued coupon income amount",
        ["PriceStep"] = "Price step",
        ["Lot"] = "Lot size",
        ["NPLtoMarketCurPrice"] = "Nominal profit or loss",
        ["NPLPercent"] = "Nominal profit or loss percent",
        ["PlanLong"] = "Planned long position",
        ["PlanShort"] = "Planned short position"
    };

    /// <summary>
    /// Creates parsing behavior for positions. Usage example: var entries = new JsonPositionsEntries(payload, accountId).
    /// </summary>
    public JsonPositionsEntries(string payload, long accountId)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
        _accountId = accountId;
    }

    /// <summary>
    /// Returns positions entries json filtered by account with descriptions. Usage example: string json = entries.Json().
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
            throw new InvalidOperationException("Account positions are missing");
        }
        return JsonSerializer.Serialize(list);
    }

    private static JsonObject Entry(JsonElement node)
    {
        JsonObject entry = new();
        entry["IdPosition"] = Field(Integer(node, "IdPosition"), _descriptions["IdPosition"]);
        entry["IdAccount"] = Field(Integer(node, "IdAccount"), _descriptions["IdAccount"]);
        entry["IdSubAccount"] = Field(Integer(node, "IdSubAccount"), _descriptions["IdSubAccount"]);
        entry["IdRazdel"] = Field(Integer(node, "IdRazdel"), _descriptions["IdRazdel"]);
        entry["IdObject"] = Field(Integer(node, "IdObject"), _descriptions["IdObject"]);
        entry["IdFiBalance"] = Field(Integer(node, "IdFiBalance"), _descriptions["IdFiBalance"]);
        entry["IdBalanceGroup"] = Field(Integer(node, "IdBalanceGroup"), _descriptions["IdBalanceGroup"]);
        entry["AssetsPercent"] = Field(Double(node, "AssetsPercent"), _descriptions["AssetsPercent"]);
        entry["PSTNKD"] = Field(Double(node, "PSTNKD"), _descriptions["PSTNKD"]);
        entry["IsMoney"] = Field(Bool(node, "IsMoney"), _descriptions["IsMoney"]);
        entry["IsRur"] = Field(Bool(node, "IsRur"), _descriptions["IsRur"]);
        entry["UchPrice"] = Field(Double(node, "UchPrice"), _descriptions["UchPrice"]);
        entry["TorgPos"] = Field(Double(node, "TorgPos"), _descriptions["TorgPos"]);
        entry["Price"] = Field(Double(node, "Price"), _descriptions["Price"]);
        entry["DailyPL"] = Field(Double(node, "DailyPL"), _descriptions["DailyPL"]);
        entry["DailyPLPercentToMarketCurPrice"] = Field(Double(node, "DailyPLPercentToMarketCurPrice"), _descriptions["DailyPLPercentToMarketCurPrice"]);
        entry["BackPos"] = Field(Double(node, "BackPos"), _descriptions["BackPos"]);
        entry["PrevQuote"] = Field(Double(node, "PrevQuote"), _descriptions["PrevQuote"]);
        entry["TrnIn"] = Field(Double(node, "TrnIn"), _descriptions["TrnIn"]);
        entry["TrnOut"] = Field(Double(node, "TrnOut"), _descriptions["TrnOut"]);
        entry["DailyBuyVolume"] = Field(Double(node, "DailyBuyVolume"), _descriptions["DailyBuyVolume"]);
        entry["DailySellVolume"] = Field(Double(node, "DailySellVolume"), _descriptions["DailySellVolume"]);
        entry["DailyBuyQuantity"] = Field(Double(node, "DailyBuyQuantity"), _descriptions["DailyBuyQuantity"]);
        entry["DailySellQuantity"] = Field(Double(node, "DailySellQuantity"), _descriptions["DailySellQuantity"]);
        entry["NKD"] = Field(Double(node, "NKD"), _descriptions["NKD"]);
        entry["PriceStep"] = Field(Double(node, "PriceStep"), _descriptions["PriceStep"]);
        entry["Lot"] = Field(Integer(node, "Lot"), _descriptions["Lot"]);
        entry["NPLtoMarketCurPrice"] = Field(Double(node, "NPLtoMarketCurPrice"), _descriptions["NPLtoMarketCurPrice"]);
        entry["NPLPercent"] = Field(Double(node, "NPLPercent"), _descriptions["NPLPercent"]);
        entry["PlanLong"] = Field(Double(node, "PlanLong"), _descriptions["PlanLong"]);
        entry["PlanShort"] = Field(Double(node, "PlanShort"), _descriptions["PlanShort"]);
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

    private static bool Bool(JsonElement node, string property)
    {
        if (!node.TryGetProperty(property, out JsonElement value))
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        if (value.ValueKind != JsonValueKind.True && value.ValueKind != JsonValueKind.False)
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        return value.GetBoolean();
    }
}
