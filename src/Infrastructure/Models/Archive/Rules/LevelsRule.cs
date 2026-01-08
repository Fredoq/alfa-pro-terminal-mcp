using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Rules;

/// <summary>
/// Builds MPV levels array from vector fields and adds it to the output object. Usage example: rule.Apply(element, output).
/// </summary>
internal sealed class LevelsRule : IJsonRule
{
    /// <summary>
    /// Creates a levels rule. Usage example: var rule = new LevelsRule().
    /// </summary>
    public LevelsRule()
    {
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
            item["Price"] = JsonValue.Create(prices[index].GetDouble());
            item["Volume"] = JsonValue.Create(volumes[index].GetInt64());
            item["VolumeAsk"] = JsonValue.Create(asks[index].GetInt64());
            list.Add(item);
        }
        root["Levels"] = list;
    }
}
