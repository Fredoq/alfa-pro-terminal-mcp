using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

/// <summary>
/// Verifies JsonInteger JSON value reader behavior. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonIntegerTests
{
    /// <summary>
    /// Ensures that JsonInteger returns the numeric property value under concurrency. Usage example: new JsonInteger(node, name).Value().
    /// </summary>
    [Fact(DisplayName = "JsonInteger returns number value")]
    public void Given_number_when_read_then_returns_value()
    {
        long number = RandomNumberGenerator.GetInt32(10_000, 99_999);
        string name = $"ключ-{Guid.NewGuid()}-φ";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = number });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonInteger item = new(node, name);
        ConcurrentBag<long> list = [];
        Parallel.For(0, 5, _ => list.Add(item.Value()));
        bool result = list.All(value => value == number);
        Assert.True(result, "JsonInteger does not return number value");
    }

    /// <summary>
    /// Ensures that JsonInteger throws when property is missing. Usage example: Assert.Throws(() => new JsonInteger(node, name).Value()).
    /// </summary>
    [Fact(DisplayName = "JsonInteger throws when property is missing")]
    public void Given_missing_property_when_read_then_throws()
    {
        long number = RandomNumberGenerator.GetInt32(10_000, 99_999);
        string name = $"ключ-{Guid.NewGuid()}-φ";
        string miss = $"нет-{Guid.NewGuid()}-λ";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = number });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonInteger item = new(node, miss);
        Assert.Throws<InvalidOperationException>(() => item.Value());
    }
}
