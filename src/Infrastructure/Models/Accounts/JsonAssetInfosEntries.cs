using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds asset infos from JSON payload. Usage example: string json = new JsonAssetInfosEntries(payload, filter).Json().
/// </summary>
internal sealed class JsonAssetInfosEntries : IAssetInfosEntries
{
    private readonly string _payload;
    private readonly IAssetFilter _filter;
    private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>
    {
        ["IdObject"] = "Asset identifier",
        ["Ticker"] = "Exchange ticker",
        ["ISIN"] = "International security identifier",
        ["Name"] = "Asset name",
        ["Description"] = "Asset description",
        ["Nominal"] = "Nominal value",
        ["IdObjectType"] = "Asset type identifier",
        ["IdObjectGroup"] = "Asset group identifier",
        ["IdObjectBase"] = "Base asset identifier",
        ["IdObjectFaceUnit"] = "Face value currency identifier",
        ["MatDateObject"] = "Expiration date of asset",
        ["Instruments"] = "Financial instruments list",
        ["IdFi"] = "Financial instrument identifier",
        ["RCode"] = "Portfolio code",
        ["IsLiquid"] = "Liquidity flag",
        ["IdMarketBoard"] = "Market identifier"
    };

    /// <summary>
    /// Creates parsing behavior for asset infos. Usage example: var infos = new JsonAssetInfosEntries(payload, filter).
    /// </summary>
    public JsonAssetInfosEntries(string payload, IAssetFilter filter)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    /// <summary>
    /// Returns asset infos json filtered by identifiers with descriptions. Usage example: string json = infos.Json().
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
            if (!_filter.Filtered(node))
            {
                continue;
            }
            list.Add(Entry(node));
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Asset infos are missing");
        }
        return JsonSerializer.Serialize(list);
    }

    /// <summary>
    /// Builds described JSON entry from payload node. Usage example: JsonObject entry = Entry(node);.
    /// </summary>
    private static JsonObject Entry(JsonElement node)
    {
        JsonObject entry = new();
        entry["IdObject"] = Field(new JsonInteger(node, "IdObject").Value(), _descriptions["IdObject"]);
        entry["Ticker"] = Field(new JsonString(node, "Ticker").Value(), _descriptions["Ticker"]);
        entry["ISIN"] = Field(new JsonString(node, "ISIN").Value(), _descriptions["ISIN"]);
        entry["Name"] = Field(new JsonString(node, "Name").Value(), _descriptions["Name"]);
        entry["Description"] = Field(new JsonString(node, "Description").Value(), _descriptions["Description"]);
        entry["Nominal"] = Field(new JsonDouble(node, "Nominal").Value(), _descriptions["Nominal"]);
        entry["IdObjectType"] = Field(new JsonInteger(node, "IdObjectType").Value(), _descriptions["IdObjectType"]);
        entry["IdObjectGroup"] = Field(new JsonInteger(node, "IdObjectGroup").Value(), _descriptions["IdObjectGroup"]);
        entry["IdObjectBase"] = Field(new JsonInteger(node, "IdObjectBase").Value(), _descriptions["IdObjectBase"]);
        entry["IdObjectFaceUnit"] = Field(new JsonInteger(node, "IdObjectFaceUnit").Value(), _descriptions["IdObjectFaceUnit"]);
        entry["MatDateObject"] = Field(new JsonString(node, "MatDateObject").Value(), _descriptions["MatDateObject"]);
        entry["Instruments"] = Instruments(node);
        return entry;
    }

    /// <summary>
    /// Builds instruments collection with descriptions. Usage example: JsonArray array = Instruments(node);.
    /// </summary>
    private static JsonArray Instruments(JsonElement node)
    {
        JsonArray array = [];
        if (!node.TryGetProperty("Instruments", out JsonElement instruments) || instruments.ValueKind != JsonValueKind.Array)
        {
            return array;
        }
        foreach (JsonElement instrument in instruments.EnumerateArray())
        {
            JsonObject item = new();
            item["IdFi"] = Field(new JsonInteger(instrument, "IdFi").Value(), _descriptions["IdFi"]);
            item["RCode"] = Field(new JsonString(instrument, "RCode").Value(), _descriptions["RCode"]);
            item["IsLiquid"] = Field(new JsonBool(instrument, "IsLiquid").Value(), _descriptions["IsLiquid"]);
            item["IdMarketBoard"] = Field(new JsonInteger(instrument, "IdMarketBoard").Value(), _descriptions["IdMarketBoard"]);
            array.Add(item);
        }
        return array;
    }

    /// <summary>
    /// Creates described field node. Usage example: JsonObject field = Field(value, description);.
    /// </summary>
    private static JsonObject Field<T>(T value, string description) where T : notnull
    {
        JsonObject field = new();
        field["value"] = JsonValue.Create(value);
        field["description"] = description;
        return field;
    }
}
