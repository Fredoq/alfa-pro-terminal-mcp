using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Archive;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Builds archive candles JSON with field descriptions. Usage example: string json = new JsonArchiveEntries(payload).Json().
/// </summary>
internal sealed class JsonArchiveEntries : IArchiveEntries
{
    private readonly string _payload;
    private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>
    {
        ["Open"] = "Opening price",
        ["Close"] = "Closing price",
        ["Low"] = "Lowest price in timeframe",
        ["High"] = "Highest price in timeframe",
        ["Volume"] = "Traded volume in timeframe",
        ["VolumeAsk"] = "Ask volume in timeframe",
        ["OpenInt"] = "Open interest for futures",
        ["Time"] = "Candle timestamp",
        ["Levels"] = "Price levels for MPV candle",
        ["Price"] = "Price at level",
        ["QtyVolume"] = "Volume at level in timeframe",
        ["QtyVolumeAsk"] = "Ask volume at level in timeframe"
    };

    public JsonArchiveEntries(string payload)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
    }

    public string Json()
    {
        using JsonDocument document = JsonDocument.Parse(_payload);
        JsonElement root = document.RootElement;
        JsonArray list = [];
        if (root.TryGetProperty("OHLCV", out JsonElement ohlcv) && ohlcv.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement node in ohlcv.EnumerateArray())
            {
                list.Add(Ohlcv(node));
            }
        }
        else if (root.TryGetProperty("MPV", out JsonElement mpv) && mpv.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement node in mpv.EnumerateArray())
            {
                list.Add(Mpv(node));
            }
        }
        else
        {
            throw new InvalidOperationException("Archive candles are missing");
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Archive candles are missing");
        }
        return JsonSerializer.Serialize(list);
    }

    private static JsonObject Ohlcv(JsonElement node)
    {
        JsonObject entry = new();
        entry["Open"] = Field(new JsonDouble(node, "Open").Value(), _descriptions["Open"]);
        entry["Close"] = Field(new JsonDouble(node, "Close").Value(), _descriptions["Close"]);
        entry["Low"] = Field(new JsonDouble(node, "Low").Value(), _descriptions["Low"]);
        entry["High"] = Field(new JsonDouble(node, "High").Value(), _descriptions["High"]);
        entry["Volume"] = Field(new JsonInteger(node, "Volume").Value(), _descriptions["Volume"]);
        entry["VolumeAsk"] = Field(new JsonInteger(node, "VolumeAsk").Value(), _descriptions["VolumeAsk"]);
        entry["OpenInt"] = Field(new JsonInteger(node, "OpenInt").Value(), _descriptions["OpenInt"]);
        entry["Time"] = Field(new JsonString(node, "DT").Value(), _descriptions["Time"]);
        return entry;
    }

    private static JsonObject Mpv(JsonElement node)
    {
        JsonObject entry = new();
        entry["Open"] = Field(new JsonDouble(node, "Open").Value(), _descriptions["Open"]);
        entry["Close"] = Field(new JsonDouble(node, "Close").Value(), _descriptions["Close"]);
        entry["Time"] = Field(new JsonString(node, "DT").Value(), _descriptions["Time"]);
        entry["Levels"] = Levels(node);
        return entry;
    }

    private static JsonArray Levels(JsonElement node)
    {
        if (!node.TryGetProperty("Prices", out JsonElement prices) || prices.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Prices is missing");
        }
        if (!node.TryGetProperty("Volumes", out JsonElement volumes) || volumes.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Volumes is missing");
        }
        if (!node.TryGetProperty("AskVolumes", out JsonElement asks) || asks.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("AskVolumes is missing");
        }
        int count = prices.GetArrayLength();
        if (volumes.GetArrayLength() != count || asks.GetArrayLength() != count)
        {
            throw new InvalidOperationException("Volume lengths are inconsistent");
        }
        JsonArray array = [];
        for (int index = 0; index < count; index++)
        {
            JsonObject item = new();
            item["Price"] = Field(prices[index].GetDouble(), _descriptions["Price"]);
            item["Volume"] = Field(volumes[index].GetInt64(), _descriptions["QtyVolume"]);
            item["VolumeAsk"] = Field(asks[index].GetInt64(), _descriptions["QtyVolumeAsk"]);
            array.Add(item);
        }
        return array;
    }

    private static JsonObject Field<T>(T value, string description) where T : notnull
    {
        JsonObject field = new();
        field["value"] = JsonValue.Create(value);
        field["description"] = description;
        return field;
    }
}
