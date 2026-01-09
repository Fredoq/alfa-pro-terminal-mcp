using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
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
                    ArchiveTool tool = new(terminal, logger);
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
