using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies accounts entries transformation from router payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AccountsEntriesTests
{
    /// <summary>
    /// Ensures that accounts entries extract account id and type. Usage example: new SchemaEntries(...).Json().
    /// </summary>
    [Fact(DisplayName = "Accounts entries extract account identifiers and types")]
    public void Given_json_with_accounts_when_parsed_then_extracts_fields()
    {
        long account = RandomNumberGenerator.GetInt32(10_000, 100_000);
        int code = RandomNumberGenerator.GetInt32(1, 4);
        string payload = $"{{\"Data\":[{{\"IdAccount\":{account},\"IIAType\":{code},\"Name\":\"ночь\"}}]}}";
        SchemaEntries entries = new(new PayloadArrayEntries(payload), new AccountsSchema());
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        JsonElement node = root[0];
        bool result = node.GetProperty("AccountId").GetInt64() == account && node.GetProperty("IIAType").GetInt32() == code;
        Assert.True(result, "Accounts entries do not extract account identifiers and types");
    }

    /// <summary>
    /// Checks that accounts entries yield consistent output in parallel calls. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Accounts entries remain consistent under concurrency")]
    public void Given_concurrent_calls_when_parsed_then_outputs_identical()
    {
        int code = RandomNumberGenerator.GetInt32(2, 7);
        long account = RandomNumberGenerator.GetInt32(11_111, 100_000);
        string payload = $"{{\"Data\":[{{\"IdAccount\":{account},\"IIAType\":{code}}}]}}";
        SchemaEntries entries = new(new PayloadArrayEntries(payload), new AccountsSchema());
        ConcurrentBag<string> results = new();
        Parallel.For(0, 5, _ => results.Add(entries.Json()));
        string sample = results.First();
        bool identical = results.All(item => item == sample);
        Assert.True(identical, "Accounts entries do not remain consistent under concurrency");
    }

    /// <summary>
    /// Confirms that accounts entries fail when account type is missing. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "Accounts entries throw when account type is missing")]
    public void Given_missing_type_when_parsed_then_throws()
    {
        long account = RandomNumberGenerator.GetInt32(101, 1_000);
        string payload = $"{{\"Data\":[{{\"IdAccount\":{account}}}]}}";
        SchemaEntries entries = new(new PayloadArrayEntries(payload), new AccountsSchema());
        Assert.Throws<InvalidOperationException>(() => entries.Json());
    }
}
