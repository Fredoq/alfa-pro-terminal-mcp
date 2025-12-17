using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Builds MPV levels array from vector fields and adds it to the output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class LevelsRule : IJsonRule
{
    private readonly string _textPrice;
    private readonly string _textVolume;
    private readonly string _textAsk;

    /// <summary>
    /// Creates a levels rule with described level fields. Usage example: var rule = new LevelsRule(descriptions).
    /// </summary>
    public LevelsRule()
    {
        ArchiveDescriptions text = new();
        _textPrice = text.Text("Price");
        _textVolume = text.Text("QtyVolume");
        _textAsk = text.Text("QtyVolumeAsk");
    }

    /// <summary>
    /// Applies the levels rule by reading Prices, Volumes, and AskVolumes arrays. Usage example: rule.Apply(element, output).
    /// </summary>
    public void Apply(JsonElement node, JsonObject root)
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
        JsonArray list = [];
        for (int index = 0; index < count; index++)
        {
            JsonObject item = new();
            JsonObject price = new();
            price["value"] = JsonValue.Create(prices[index].GetDouble());
            price["description"] = _textPrice;
            item["Price"] = price;
            JsonObject volume = new()
            {
                ["value"] = JsonValue.Create(volumes[index].GetInt64()),
                ["description"] = _textVolume
            };
            item["Volume"] = volume;
            JsonObject ask = new();
            ask["value"] = JsonValue.Create(asks[index].GetInt64());
            ask["description"] = _textAsk;
            item["VolumeAsk"] = ask;
            list.Add(item);
        }
        root["Levels"] = list;
    }
}

