using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies JsonAccountBalance transformation from router payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonAccountBalanceTests
{
    /// <summary>
    /// Ensures that JsonAccountBalance filters by account and adds descriptions. Usage example: new JsonAccountBalance(payload, accountId).Json().
    /// </summary>
    [Fact(DisplayName = "JsonAccountBalance returns described fields for a matching account")]
    public void Given_payload_with_multiple_accounts_when_parsed_then_filters_and_describes()
    {
        long account = RandomNumberGenerator.GetInt32(10_000, 99_999);
        long sub = RandomNumberGenerator.GetInt32(100_000, 199_999);
        long other = account + RandomNumberGenerator.GetInt32(3, 9);
        int group = RandomNumberGenerator.GetInt32(1, 4);
        int amount = RandomNumberGenerator.GetInt32(50, 150);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdAccount = account,
                    IdSubAccount = sub,
                    IdRazdelGroup = group,
                    DataId = sub * 8 + group,
                    MarginInitial = amount,
                    MarginMinimum = amount + 1,
                    MarginRequirement = amount + 2,
                    Money = amount + 3,
                    MoneyInitial = amount + 4,
                    Balance = amount + 5,
                    PrevBalance = amount + 6,
                    PortfolioCost = amount + 7,
                    LiquidBalance = amount + 8,
                    Requirements = amount + 9,
                    ImmediateRequirements = amount + 10,
                    NPL = amount + 11,
                    DailyPL = amount + 12,
                    NPLPercent = amount + 13,
                    DailyPLPercent = amount + 14,
                    NKD = amount + 15,
                    NameBalanceGroup = "баланс-ζ"
                },
                new
                {
                    IdAccount = other,
                    IdSubAccount = sub + 1,
                    IdRazdelGroup = group,
                    DataId = (sub + 1) * 8 + group,
                    MarginInitial = amount,
                    MarginMinimum = amount,
                    MarginRequirement = amount,
                    Money = amount,
                    MoneyInitial = amount,
                    Balance = amount,
                    PrevBalance = amount,
                    PortfolioCost = amount,
                    LiquidBalance = amount,
                    Requirements = amount,
                    ImmediateRequirements = amount,
                    NPL = amount,
                    DailyPL = amount,
                    NPLPercent = amount,
                    DailyPLPercent = amount,
                    NKD = amount
                }
            }
        });
        JsonAccountBalance balance = new(payload, account);
        string json = balance.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        bool result = entry.GetProperty("IdAccount").GetProperty("value").GetInt64() == account && entry.GetProperty("IdAccount").GetProperty("description").GetString()?.Length > 0 && entry.TryGetProperty("NPLPercent", out _);
        Assert.True(result, "JsonAccountBalance does not filter and describe balance fields");
    }

    /// <summary>
    /// Confirms that JsonAccountBalance fails when the requested account is absent. Usage example: new JsonAccountBalance(payload, accountId).Json().
    /// </summary>
    [Fact(DisplayName = "JsonAccountBalance throws when target account is missing")]
    public void Given_payload_without_target_account_when_parsed_then_throws()
    {
        long account = RandomNumberGenerator.GetInt32(200_000, 299_999);
        int group = RandomNumberGenerator.GetInt32(1, 4);
        int amount = RandomNumberGenerator.GetInt32(11, 99);
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdAccount = account + 1,
                    IdSubAccount = account + 2,
                    IdRazdelGroup = group,
                    DataId = (account + 2) * 8 + group,
                    MarginInitial = amount,
                    MarginMinimum = amount,
                    MarginRequirement = amount,
                    Money = amount,
                    MoneyInitial = amount,
                    Balance = amount,
                    PrevBalance = amount,
                    PortfolioCost = amount,
                    LiquidBalance = amount,
                    Requirements = amount,
                    ImmediateRequirements = amount,
                    NPL = amount,
                    DailyPL = amount,
                    NPLPercent = amount,
                    DailyPLPercent = amount,
                    NKD = amount
                }
            }
        });
        JsonAccountBalance balance = new(payload, account);
        Assert.Throws<InvalidOperationException>(() => balance.Json());
    }
}
