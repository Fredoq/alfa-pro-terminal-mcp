using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

/// <summary>
/// Verifies JsonDouble JSON value reader behavior. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonDoubleTests
{
    /// <summary>
    /// Ensures that JsonDouble returns the numeric property value under concurrency. Usage example: new JsonDouble(node, name).Value().
    /// </summary>
    [Fact(DisplayName = "JsonDouble returns number value")]
    public void Given_number_when_read_then_returns_value()
    {
        double number = RandomNumberGenerator.GetInt32(1, 1000) / 10d;
        string name = $"ключ-{Guid.NewGuid()}-δ";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = number });
        JsonNode node = JsonNode.Parse(json) ?? throw new InvalidOperationException("Payload is missing");
        JsonObject root = node.AsObject();
        JsonDouble item = new(root, name);
        ConcurrentBag<double> list = [];
        Parallel.For(0, 5, _ => list.Add(item.Value()));
        bool result = list.All(value => Math.Abs(value - number) < 0.0000001d);
        Assert.True(result, "JsonDouble does not return number value");
    }

    /// <summary>
    /// Ensures that JsonDouble throws when property is missing. Usage example: Assert.Throws(() => new JsonDouble(node, name).Value()).
    /// </summary>
    [Fact(DisplayName = "JsonDouble throws when property is missing")]
    public void Given_missing_property_when_read_then_throws()
    {
        double number = RandomNumberGenerator.GetInt32(1, 1000) / 10d;
        string name = $"ключ-{Guid.NewGuid()}-δ";
        string miss = $"нет-{Guid.NewGuid()}-ψ";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = number });
        JsonNode node = JsonNode.Parse(json) ?? throw new InvalidOperationException("Payload is missing");
        JsonObject root = node.AsObject();
        JsonDouble item = new(root, miss);
        Assert.Throws<InvalidOperationException>(() => item.Value());
    }
}
