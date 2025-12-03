using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

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
        entry["IdObject"] = Field(Integer(node, "IdObject"), _descriptions["IdObject"]);
        entry["Ticker"] = Field(String(node, "Ticker"), _descriptions["Ticker"]);
        entry["ISIN"] = Field(String(node, "ISIN"), _descriptions["ISIN"]);
        entry["Name"] = Field(String(node, "Name"), _descriptions["Name"]);
        entry["Description"] = Field(String(node, "Description"), _descriptions["Description"]);
        entry["Nominal"] = Field(Double(node, "Nominal"), _descriptions["Nominal"]);
        entry["IdObjectType"] = Field(Integer(node, "IdObjectType"), _descriptions["IdObjectType"]);
        entry["IdObjectGroup"] = Field(Integer(node, "IdObjectGroup"), _descriptions["IdObjectGroup"]);
        entry["IdObjectBase"] = Field(Integer(node, "IdObjectBase"), _descriptions["IdObjectBase"]);
        entry["IdObjectFaceUnit"] = Field(Integer(node, "IdObjectFaceUnit"), _descriptions["IdObjectFaceUnit"]);
        entry["MatDateObject"] = Field(String(node, "MatDateObject"), _descriptions["MatDateObject"]);
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
            item["IdFi"] = Field(Integer(instrument, "IdFi"), _descriptions["IdFi"]);
            item["RCode"] = Field(String(instrument, "RCode"), _descriptions["RCode"]);
            item["IsLiquid"] = Field(Bool(instrument, "IsLiquid"), _descriptions["IsLiquid"]);
            item["IdMarketBoard"] = Field(Integer(instrument, "IdMarketBoard"), _descriptions["IdMarketBoard"]);
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

    /// <summary>
    /// Extracts integer property from payload. Usage example: long id = Integer(node, "IdObject");.
    /// </summary>
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

    /// <summary>
    /// Extracts string property from payload. Usage example: string ticker = String(node, "Ticker");.
    /// </summary>
    private static string String(JsonElement node, string property)
    {
        if (!node.TryGetProperty(property, out JsonElement value))
        {
            throw new InvalidOperationException($"{property} is missing");
        }
        if (value.ValueKind == JsonValueKind.String)
        {
            return value.GetString() ?? string.Empty;
        }
        if (value.ValueKind == JsonValueKind.Null)
        {
            return string.Empty;
        }
        throw new InvalidOperationException($"{property} is missing");
    }

    /// <summary>
    /// Extracts double property from payload. Usage example: double nominal = Double(node, "Nominal");.
    /// </summary>
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

    /// <summary>
    /// Extracts boolean property from payload. Usage example: bool liquid = Bool(node, "IsLiquid");.
    /// </summary>
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
