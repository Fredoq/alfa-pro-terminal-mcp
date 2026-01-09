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
    /// Applies the levels rule by reading Prices, Volumes, and AskVolumes arrays. Usage example: rule.Apply(item, output).
    /// </summary>
    public void Apply(JsonObject node, JsonObject root)
    {
        if (!node.TryGetPropertyValue("Prices", out JsonNode? price) || price is null)
        {
            throw new InvalidOperationException("Prices is missing");
        }
        if (!node.TryGetPropertyValue("Volumes", out JsonNode? volume) || volume is null)
        {
            throw new InvalidOperationException("Volumes is missing");
        }
        if (!node.TryGetPropertyValue("AskVolumes", out JsonNode? ask) || ask is null)
        {
            throw new InvalidOperationException("AskVolumes is missing");
        }
        JsonArray prices;
        try
        {
            prices = price.AsArray();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("Prices is missing");
        }
        JsonArray volumes;
        try
        {
            volumes = volume.AsArray();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("Volumes is missing");
        }
        JsonArray asks;
        try
        {
            asks = ask.AsArray();
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("AskVolumes is missing");
        }
        int count = prices.Count;
        if (volumes.Count != count || asks.Count != count)
        {
            throw new InvalidOperationException("Volume lengths are inconsistent");
        }
        JsonArray list = [];
        for (int index = 0; index < count; index++)
        {
            JsonNode? item = prices[index];
            JsonNode? size = volumes[index];
            JsonNode? side = asks[index];
            if (item is null || size is null || side is null)
            {
                throw new InvalidOperationException("Level value is missing");
            }
            JsonObject level = new();
            level["Price"] = JsonValue.Create(item.GetValue<double>());
            level["Volume"] = JsonValue.Create(size.GetValue<long>());
            level["VolumeAsk"] = JsonValue.Create(side.GetValue<long>());
            list.Add(level);
        }
        root["Levels"] = list;
    }
}
