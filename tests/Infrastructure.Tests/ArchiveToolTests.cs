using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for archive tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ArchiveToolTests
{
    /// <summary>
    /// Ensures that archive tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Archive tool returns structured content matching output schema")]
    public async Task Archive_tool_returns_structured_content_matching_output_schema()
    {
        int count = RandomNumberGenerator.GetInt32(2, 5);
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            Task<bool>[] tasks = new Task<bool>[count];
            for (int index = 0; index < count; index++)
            {
                tasks[index] = Task.Run(async () =>
                {
                    long id = RandomNumberGenerator.GetInt32(10_000, 90_000);
                    string note = $"свеча-{Guid.NewGuid()}-ъ";
                    string payload = JsonSerializer.Serialize(new
                    {
                        OHLCV = new object[]
                        {
                            new
                            {
                                Open = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Close = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Low = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                High = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Volume = RandomNumberGenerator.GetInt32(1, 10_000),
                                VolumeAsk = RandomNumberGenerator.GetInt32(1, 10_000),
                                OpenInt = RandomNumberGenerator.GetInt32(1, 10_000),
                                DT = DateTime.UtcNow.AddMinutes(-RandomNumberGenerator.GetInt32(1, 100)).ToString("O"),
                                Note = note
                            }
                        }
                    });
                    await using ArchiveSocketFake terminal = new(payload, false);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsArchive(terminal, logger), new Tool { Name = "history", Title = "Archive candles", Description = "Returns archive candles for given instrument, candle type, interval, period, first day and last day.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"candles":{"type":"array","description":"Archive candles for the requested instrument and interval","items":{"oneOf":[{"type":"object","properties":{"Open":{"type":"number","description":"Opening price"},"Close":{"type":"number","description":"Closing price"},"Low":{"type":"number","description":"Lowest price in timeframe"},"High":{"type":"number","description":"Highest price in timeframe"},"Volume":{"type":"integer","description":"Traded volume in timeframe"},"VolumeAsk":{"type":"integer","description":"Ask volume in timeframe"},"OpenInt":{"type":"integer","description":"Open interest for futures"},"Time":{"type":"string","description":"Candle timestamp"}},"required":["Open","Close","Low","High","Volume","VolumeAsk","OpenInt","Time"],"additionalProperties":false},{"type":"object","properties":{"Open":{"type":"number","description":"Opening price"},"Close":{"type":"number","description":"Closing price"},"Time":{"type":"string","description":"Candle timestamp"},"Levels":{"type":"array","description":"Price levels for MPV candle","items":{"type":"object","properties":{"Price":{"type":"number","description":"Price at level"},"Volume":{"type":"integer","description":"Volume at level in timeframe"},"VolumeAsk":{"type":"integer","description":"Ask volume at level in timeframe"}},"required":["Price","Volume","VolumeAsk"],"additionalProperties":false}}},"required":["Open","Close","Time","Levels"],"additionalProperties":false}]}}},"required":["candles"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idFi":{"type":"integer","description":"Financial instrument identifier"},"candleType":{"type":"integer","description":"Candle kind: 0 for OHLCV, 2 for MPV"},"interval":{"type":"string","description":"Timeframe unit: second, minute, hour, day, week or month"},"period":{"type":"integer","description":"Interval multiplier matching the interval unit"},"firstDay":{"type":"string","format":"date-time","description":"First requested trading day inclusive"},"lastDay":{"type":"string","format":"date-time","description":"Last requested trading day inclusive"}},"required":["idFi","candleType","interval","period","firstDay","lastDay"]}"""))));
                    DateTime first = DateTime.UtcNow.Date.AddDays(-RandomNumberGenerator.GetInt32(3, 10));
                    DateTime last = DateTime.UtcNow.Date.AddDays(-RandomNumberGenerator.GetInt32(1, 2));
                    Dictionary<string, JsonElement> data = new()
                    {
                        ["idFi"] = JsonSerializer.SerializeToElement(id),
                        ["candleType"] = JsonSerializer.SerializeToElement(0),
                        ["interval"] = JsonSerializer.SerializeToElement("day"),
                        ["period"] = JsonSerializer.SerializeToElement(1),
                        ["firstDay"] = JsonSerializer.SerializeToElement(first),
                        ["lastDay"] = JsonSerializer.SerializeToElement(last)
                    };
                    using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
                    CallToolResult result = await tool.Result(data, source.Token);
                    JsonNode node = result.StructuredContent ?? throw new InvalidOperationException("Structured content is missing");
                    JsonElement schema = tool.Tool().OutputSchema ?? throw new InvalidOperationException("Output schema is missing");
                    SchemaMatch probe = new();
                    return probe.Match(node, schema);
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Archive tool output does not match schema");
    }
}
