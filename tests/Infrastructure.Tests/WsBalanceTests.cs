using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Verifies WsBalance behavior for balance retrieval. Usage example: executed by xUnit runner.
/// </summary>
public sealed class WsBalanceTests
{
    /// <summary>
    /// Ensures that WsBalance returns JSON containing requested account balance. Usage example: await balance.Balance(id, token).
    /// </summary>
    [Fact(DisplayName = "WsBalance returns balance json for matching account")]
    public async Task Given_balance_response_when_requested_then_returns_json()
    {
        long account = RandomNumberGenerator.GetInt32(50_000, 80_000);
        int group = RandomNumberGenerator.GetInt32(1, 4);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdAccount = account,
                    IdSubAccount = account + 11,
                    IdRazdelGroup = group,
                    DataId = (account + 11) * 8 + group,
                    MarginInitial = 1.0,
                    MarginMinimum = 2.0,
                    MarginRequirement = 3.0,
                    Money = 4.0,
                    MoneyInitial = 5.0,
                    Balance = 6.0,
                    PrevBalance = 7.0,
                    PortfolioCost = 8.0,
                    LiquidBalance = 9.0,
                    Requirements = 10.0,
                    ImmediateRequirements = 11.0,
                    NPL = 12.0,
                    DailyPL = 13.0,
                    NPLPercent = 14.0,
                    DailyPLPercent = 15.0,
                    NKD = 16.0
                }
            }
        });
        await using BalanceSocketFake socket = new(payload);
        LoggerFake logger = new();
        WsBalance balance = new(socket, logger);
        string json = (await balance.Balance(account)).StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        bool result = entry.GetProperty("IdAccount").GetInt64() == account;
        Assert.True(result, "WsBalance does not return balance json for matching account");
    }

    /// <summary>
    /// Confirms that WsBalance fails when requested account is absent. Usage example: await balance.Balance(id, token).
    /// </summary>
    [Fact(DisplayName = "WsBalance throws when account balance is missing")]
    public async Task Given_response_without_target_account_when_requested_then_throws()
    {
        long account = RandomNumberGenerator.GetInt32(81_000, 90_000);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdAccount = account + 1,
                    IdSubAccount = account + 2,
                    IdRazdelGroup = 1,
                    DataId = (account + 2) * 8 + 1,
                    MarginInitial = 1.0,
                    MarginMinimum = 1.0,
                    MarginRequirement = 1.0,
                    Money = 1.0,
                    MoneyInitial = 1.0,
                    Balance = 1.0,
                    PrevBalance = 1.0,
                    PortfolioCost = 1.0,
                    LiquidBalance = 1.0,
                    Requirements = 1.0,
                    ImmediateRequirements = 1.0,
                    NPL = 1.0,
                    DailyPL = 1.0,
                    NPLPercent = 1.0,
                    DailyPLPercent = 1.0,
                    NKD = 1.0
                }
            }
        });
        await using BalanceSocketFake socket = new(payload);
        LoggerFake logger = new();
        WsBalance balance = new(socket, logger);
        Task<string> action = Task.Run(async () => (await balance.Balance(account)).StructuredContent().ToJsonString());
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await action);
    }
}
