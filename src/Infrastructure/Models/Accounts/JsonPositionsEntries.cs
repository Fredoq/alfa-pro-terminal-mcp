using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds position entries from JSON payload. Usage example: string json = new JsonPositionsEntries(payload, accountId).Json().
/// </summary>
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
            long account = new JsonInteger(node, "IdAccount").Value();
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
        entry["IdPosition"] = Field(new JsonInteger(node, "IdPosition").Value(), _descriptions["IdPosition"]);
        entry["IdAccount"] = Field(new JsonInteger(node, "IdAccount").Value(), _descriptions["IdAccount"]);
        entry["IdSubAccount"] = Field(new JsonInteger(node, "IdSubAccount").Value(), _descriptions["IdSubAccount"]);
        entry["IdRazdel"] = Field(new JsonInteger(node, "IdRazdel").Value(), _descriptions["IdRazdel"]);
        entry["IdObject"] = Field(new JsonInteger(node, "IdObject").Value(), _descriptions["IdObject"]);
        entry["IdFiBalance"] = Field(new JsonInteger(node, "IdFiBalance").Value(), _descriptions["IdFiBalance"]);
        entry["IdBalanceGroup"] = Field(new JsonInteger(node, "IdBalanceGroup").Value(), _descriptions["IdBalanceGroup"]);
        entry["AssetsPercent"] = Field(new JsonDouble(node, "AssetsPercent").Value(), _descriptions["AssetsPercent"]);
        entry["PSTNKD"] = Field(new JsonDouble(node, "PSTNKD").Value(), _descriptions["PSTNKD"]);
        entry["IsMoney"] = Field(new JsonBool(node, "IsMoney").Value(), _descriptions["IsMoney"]);
        entry["IsRur"] = Field(new JsonBool(node, "IsRur").Value(), _descriptions["IsRur"]);
        entry["UchPrice"] = Field(new JsonDouble(node, "UchPrice").Value(), _descriptions["UchPrice"]);
        entry["TorgPos"] = Field(new JsonDouble(node, "TorgPos").Value(), _descriptions["TorgPos"]);
        entry["Price"] = Field(new JsonDouble(node, "Price").Value(), _descriptions["Price"]);
        entry["DailyPL"] = Field(new JsonDouble(node, "DailyPL").Value(), _descriptions["DailyPL"]);
        entry["DailyPLPercentToMarketCurPrice"] = Field(new JsonDouble(node, "DailyPLPercentToMarketCurPrice").Value(), _descriptions["DailyPLPercentToMarketCurPrice"]);
        entry["BackPos"] = Field(new JsonDouble(node, "BackPos").Value(), _descriptions["BackPos"]);
        entry["PrevQuote"] = Field(new JsonDouble(node, "PrevQuote").Value(), _descriptions["PrevQuote"]);
        entry["TrnIn"] = Field(new JsonDouble(node, "TrnIn").Value(), _descriptions["TrnIn"]);
        entry["TrnOut"] = Field(new JsonDouble(node, "TrnOut").Value(), _descriptions["TrnOut"]);
        entry["DailyBuyVolume"] = Field(new JsonDouble(node, "DailyBuyVolume").Value(), _descriptions["DailyBuyVolume"]);
        entry["DailySellVolume"] = Field(new JsonDouble(node, "DailySellVolume").Value(), _descriptions["DailySellVolume"]);
        entry["DailyBuyQuantity"] = Field(new JsonDouble(node, "DailyBuyQuantity").Value(), _descriptions["DailyBuyQuantity"]);
        entry["DailySellQuantity"] = Field(new JsonDouble(node, "DailySellQuantity").Value(), _descriptions["DailySellQuantity"]);
        entry["NKD"] = Field(new JsonDouble(node, "NKD").Value(), _descriptions["NKD"]);
        entry["PriceStep"] = Field(new JsonDouble(node, "PriceStep").Value(), _descriptions["PriceStep"]);
        entry["Lot"] = Field(new JsonInteger(node, "Lot").Value(), _descriptions["Lot"]);
        entry["NPLtoMarketCurPrice"] = Field(new JsonDouble(node, "NPLtoMarketCurPrice").Value(), _descriptions["NPLtoMarketCurPrice"]);
        entry["NPLPercent"] = Field(new JsonDouble(node, "NPLPercent").Value(), _descriptions["NPLPercent"]);
        entry["PlanLong"] = Field(new JsonDouble(node, "PlanLong").Value(), _descriptions["PlanLong"]);
        entry["PlanShort"] = Field(new JsonDouble(node, "PlanShort").Value(), _descriptions["PlanShort"]);
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
