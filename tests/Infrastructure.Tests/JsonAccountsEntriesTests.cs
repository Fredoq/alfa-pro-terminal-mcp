namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;

/// <summary>
/// Verifies JsonAccountsEntries transformation from router payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonAccountsEntriesTests
{
    /// <summary>
    /// Ensures that JsonAccountsEntries extracts account id and type. Usage example: new JsonAccountsEntries(payload).Json().
    /// </summary>
    [Fact(DisplayName = "JsonAccountsEntries extracts account identifiers and types")]
    public void Given_json_with_accounts_when_parsed_then_extracts_fields()
    {
        long account = RandomNumberGenerator.GetInt32(10_000, 100_000);
        int code = RandomNumberGenerator.GetInt32(1, 4);
        string payload = $"{{\"Data\":[{{\"IdAccount\":{account},\"IIAType\":{code},\"Name\":\"ночь\"}}]}}";
        JsonAccountsEntries entries = new(payload);
        string json = entries.Json();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        JsonElement node = root[0];
        bool result = node.GetProperty("AccountId").GetInt64() == account && node.GetProperty("IIAType").GetInt32() == code;
        Assert.True(result, "JsonAccountsEntries does not extract account identifiers and types");
    }

    /// <summary>
    /// Checks that JsonAccountsEntries yields consistent output in parallel calls. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "JsonAccountsEntries remains consistent under concurrency")]
    public void Given_concurrent_calls_when_parsed_then_outputs_identical()
    {
        int code = RandomNumberGenerator.GetInt32(2, 7);
        long account = RandomNumberGenerator.GetInt32(11_111, 100_000);
        string payload = $"{{\"Data\":[{{\"IdAccount\":{account},\"IIAType\":{code}}}]}}";
        JsonAccountsEntries entries = new(payload);
        ConcurrentBag<string> results = new();
        Parallel.For(0, 5, _ => results.Add(entries.Json()));
        string sample = results.First();
        bool identical = results.All(item => item == sample);
        Assert.True(identical, "JsonAccountsEntries does not remain consistent under concurrency");
    }

    /// <summary>
    /// Confirms that JsonAccountsEntries fails when account type is missing. Usage example: entries.Json().
    /// </summary>
    [Fact(DisplayName = "JsonAccountsEntries throws when account type is missing")]
    public void Given_missing_type_when_parsed_then_throws()
    {
        long account = RandomNumberGenerator.GetInt32(101, 1_000);
        string payload = $"{{\"Data\":[{{\"IdAccount\":{account}}}]}}";
        JsonAccountsEntries entries = new(payload);
        Assert.Throws<InvalidOperationException>(() => entries.Json());
    }
}
