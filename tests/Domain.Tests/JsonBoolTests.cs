using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

/// <summary>
/// Verifies JsonBool JSON value reader behavior. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonBoolTests
{
    /// <summary>
    /// Ensures that JsonBool returns the boolean property value under concurrency. Usage example: new JsonBool(node, name).Value().
    /// </summary>
    [Fact(DisplayName = "JsonBool returns boolean value")]
    public void Given_boolean_when_read_then_returns_value()
    {
        bool flag = RandomNumberGenerator.GetInt32(0, 2) == 1;
        string name = $"ключ-{Guid.NewGuid()}-ξ";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = flag });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonBool item = new(node, name);
        ConcurrentBag<bool> list = [];
        Parallel.For(0, 5, _ => list.Add(item.Value()));
        bool result = list.All(value => value == flag);
        Assert.True(result, "JsonBool does not return boolean value");
    }

    /// <summary>
    /// Ensures that JsonBool throws when property is missing. Usage example: Assert.Throws(() => new JsonBool(node, name).Value()).
    /// </summary>
    [Fact(DisplayName = "JsonBool throws when property is missing")]
    public void Given_missing_property_when_read_then_throws()
    {
        bool flag = RandomNumberGenerator.GetInt32(0, 2) == 1;
        string name = $"ключ-{Guid.NewGuid()}-ξ";
        string miss = $"нет-{Guid.NewGuid()}-ω";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = flag });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonBool item = new(node, miss);
        Assert.Throws<InvalidOperationException>(() => item.Value());
    }
}
