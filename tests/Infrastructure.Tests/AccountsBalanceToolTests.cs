using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for account balance tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AccountsBalanceToolTests
{
    /// <summary>
    /// Ensures that account balance tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Account balance tool returns structured content matching output schema")]
    public async Task Account_balance_tool_returns_structured_content_matching_output_schema()
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
                    int group = RandomNumberGenerator.GetInt32(1, 7);
                    string note = $"баланс-{Guid.NewGuid()}-π";
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                DataId = account + group,
                                IdAccount = account,
                                IdSubAccount = account + 11,
                                IdRazdelGroup = group,
                                MarginInitial = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                MarginMinimum = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                MarginRequirement = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Money = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                MoneyInitial = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Balance = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PrevBalance = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                PortfolioCost = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                LiquidBalance = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Requirements = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                ImmediateRequirements = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NPL = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyPL = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NPLPercent = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                DailyPLPercent = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                NKD = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                Note = note
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    AccountsBalancePlan plan = new();
                    McpTool tool = new(new WsBalance(terminal, logger), plan);
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
        Assert.True(match, "Account balance tool output does not match schema");
    }
}
