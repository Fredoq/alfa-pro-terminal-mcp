using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for archive candles. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class ArchivePlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"candles":{"type":"array","description":"Archive candles for the requested instrument and interval","items":{"oneOf":[{"type":"object","properties":{"Open":{"type":"number","description":"Opening price"},"Close":{"type":"number","description":"Closing price"},"Low":{"type":"number","description":"Lowest price in timeframe"},"High":{"type":"number","description":"Highest price in timeframe"},"Volume":{"type":"integer","description":"Traded volume in timeframe"},"VolumeAsk":{"type":"integer","description":"Ask volume in timeframe"},"OpenInt":{"type":"integer","description":"Open interest for futures"},"Time":{"type":"string","description":"Candle timestamp"}},"required":["Open","Close","Low","High","Volume","VolumeAsk","OpenInt","Time"],"additionalProperties":false},{"type":"object","properties":{"Open":{"type":"number","description":"Opening price"},"Close":{"type":"number","description":"Closing price"},"Time":{"type":"string","description":"Candle timestamp"},"Levels":{"type":"array","description":"Price levels for MPV candle","items":{"type":"object","properties":{"Price":{"type":"number","description":"Price at level"},"Volume":{"type":"integer","description":"Volume at level in timeframe"},"VolumeAsk":{"type":"integer","description":"Ask volume at level in timeframe"}},"required":["Price","Volume","VolumeAsk"],"additionalProperties":false}}},"required":["Open","Close","Time","Levels"],"additionalProperties":false}]}}},"required":["candles"],"additionalProperties":false}""");
        return new Tool { Name = "history", Title = "Archive candles", Description = "Returns archive candles for given instrument, candle type, interval, period, first day and last day.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}"""));
        return new MappedPayload(data, schema);
    }
}
