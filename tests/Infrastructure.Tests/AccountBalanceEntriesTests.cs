using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies account balance entries transformation from router payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AccountBalanceEntriesTests
{
    /// <summary>
    /// Ensures that balance entries filter by account and return fields. Usage example: new SchemaEntries(...).Json().
    /// </summary>
    [Fact(DisplayName = "Account balance entries return fields for a matching account")]
    public void Given_payload_with_multiple_accounts_when_parsed_then_filters()
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
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AccountScope(account), "Account balance is missing"), new AccountBalanceSchema());
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement entry = document.RootElement[0];
        bool result = entry.GetProperty("IdAccount").GetInt64() == account && entry.TryGetProperty("NPLPercent", out _);
        Assert.True(result, "Account balance entries do not filter balance fields");
    }

    /// <summary>
    /// Confirms that balance entries fail when the requested account is absent. Usage example: new SchemaEntries(...).Json().
    /// </summary>
    [Fact(DisplayName = "Account balance entries throw when target account is missing")]
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
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AccountScope(account), "Account balance is missing"), new AccountBalanceSchema());
        Assert.Throws<InvalidOperationException>(() => entries.Json());
    }
}
