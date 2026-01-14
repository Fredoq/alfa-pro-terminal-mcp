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
                    McpTool tool = new(new WsBalance(terminal, logger), new Tool { Name = "balance", Title = "Account balance", Description = "Returns account balance for the given account id.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"balances":{"type":"array","description":"Account balance entries for the requested account","items":{"type":"object","properties":{"DataId":{"type":"integer","description":"Balance identifier computed as IdSubAccount * 8 + IdRazdelGroup"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdelGroup":{"type":"integer","description":"Portfolio group code"},"MarginInitial":{"type":"number","description":"Initial margin"},"MarginMinimum":{"type":"number","description":"Minimum margin"},"MarginRequirement":{"type":"number","description":"Margin requirements"},"Money":{"type":"number","description":"Cash in rubles"},"MoneyInitial":{"type":"number","description":"Opening cash in rubles"},"Balance":{"type":"number","description":"Balance value"},"PrevBalance":{"type":"number","description":"Opening balance"},"PortfolioCost":{"type":"number","description":"Portfolio value"},"LiquidBalance":{"type":"number","description":"Liquid portfolio value"},"Requirements":{"type":"number","description":"Requirements"},"ImmediateRequirements":{"type":"number","description":"Immediate requirements"},"NPL":{"type":"number","description":"Nominal profit or loss"},"DailyPL":{"type":"number","description":"Daily profit or loss"},"NPLPercent":{"type":"number","description":"Nominal PnL percent"},"DailyPLPercent":{"type":"number","description":"Daily PnL percent"},"NKD":{"type":"number","description":"Accrued coupon income"}},"required":["DataId","IdAccount","IdSubAccount","IdRazdelGroup","MarginInitial","MarginMinimum","MarginRequirement","Money","MoneyInitial","Balance","PrevBalance","PortfolioCost","LiquidBalance","Requirements","ImmediateRequirements","NPL","DailyPL","NPLPercent","DailyPLPercent","NKD"],"additionalProperties":false}}},"required":["balances"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""))));
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
