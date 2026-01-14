using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for asset info by tickers tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AssetsTickersToolTests
{
    /// <summary>
    /// Ensures that asset info by tickers tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Asset info by tickers tool returns structured content matching output schema")]
    public async Task Asset_info_by_tickers_tool_returns_structured_content_matching_output_schema()
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
                    string ticker = $"ТКР{id}";
                    string name = $"название-{Guid.NewGuid()}-π";
                    string text = $"описание-{Guid.NewGuid()}-θ";
                    string date = DateTime.UtcNow.AddDays(-RandomNumberGenerator.GetInt32(1, 10)).ToString("O");
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdObject = id,
                                Ticker = ticker,
                                ISIN = $"ISIN{id}",
                                Name = name,
                                Description = text,
                                Nominal = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                IdObjectType = RandomNumberGenerator.GetInt32(1, 100),
                                IdObjectGroup = RandomNumberGenerator.GetInt32(1, 100),
                                IdObjectBase = RandomNumberGenerator.GetInt32(1, 100),
                                IdObjectFaceUnit = RandomNumberGenerator.GetInt32(1, 100),
                                MatDateObject = date,
                                Instruments = new object[]
                                {
                                    new
                                    {
                                        IdFi = RandomNumberGenerator.GetInt32(1, 100_000),
                                        RCode = $"код-{Guid.NewGuid()}-ж",
                                        IsLiquid = true,
                                        IdMarketBoard = RandomNumberGenerator.GetInt32(1, 100)
                                    }
                                }
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    AssetsTickersPlan plan = new();
                    McpTool tool = new(new WsAssetsInfo(terminal, logger), plan);
                    Dictionary<string, JsonElement> data = new() { ["tickers"] = JsonSerializer.SerializeToElement(new[] { ticker }) };
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
        Assert.True(match, "Asset info by tickers tool output does not match schema");
    }
}
