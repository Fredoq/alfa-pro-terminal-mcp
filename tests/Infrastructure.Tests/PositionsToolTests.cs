using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for positions tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class PositionsToolTests
{
    /// <summary>
    /// Ensures that positions tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Positions tool returns structured content matching output schema")]
    public async Task Positions_tool_returns_structured_content_matching_output_schema()
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
                    long account = RandomNumberGenerator.GetInt32(10_000, 90_000);
                    string note = $"позиция-{Guid.NewGuid()}-д";
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdPosition = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdAccount = account,
                                IdSubAccount = account + 12,
                                IdRazdel = RandomNumberGenerator.GetInt32(1, 1000),
                                IdObject = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdFiBalance = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdBalanceGroup = RandomNumberGenerator.GetInt32(1, 10_000),
                                AssetsPercent = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PSTNKD = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                IsMoney = true,
                                IsRur = false,
                                UchPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                TorgPos = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Price = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyPL = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyPLPercentToMarketCurPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                BackPos = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PrevQuote = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                TrnIn = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                TrnOut = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyBuyVolume = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailySellVolume = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyBuyQuantity = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailySellQuantity = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NKD = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PriceStep = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Lot = RandomNumberGenerator.GetInt32(1, 1000),
                                NPLtoMarketCurPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NPLPercent = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PlanLong = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PlanShort = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Note = note
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    PositionsTool tool = new(terminal, logger);
                    Dictionary<string, JsonElement> data = new() { ["accountId"] = JsonSerializer.SerializeToElement(account) };
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
        Assert.True(match, "Positions tool output does not match schema");
    }
}
