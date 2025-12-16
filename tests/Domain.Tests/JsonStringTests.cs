using System.Collections.Concurrent;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

/// <summary>
/// Verifies JsonString JSON value reader behavior. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonStringTests
{
    /// <summary>
    /// Ensures that JsonString returns the string property value under concurrency. Usage example: new JsonString(node, name).Value().
    /// </summary>
    [Fact(DisplayName = "JsonString returns string value")]
    public void Given_string_when_read_then_returns_value()
    {
        string name = $"ключ-{Guid.NewGuid()}-β";
        string text = $"значение-{Guid.NewGuid()}-π";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = text });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonString item = new(node, name);
        ConcurrentBag<string> list = [];
        Parallel.For(0, 5, _ => list.Add(item.Value()));
        bool result = list.All(value => value == text);
        Assert.True(result, "JsonString does not return string value");
    }

    /// <summary>
    /// Ensures that JsonString maps JSON null to empty string. Usage example: string value = new JsonString(node, name).Value().
    /// </summary>
    [Fact(DisplayName = "JsonString maps null to empty")]
    public void Given_null_when_read_then_returns_empty()
    {
        string name = $"ключ-{Guid.NewGuid()}-β";
        string json = JsonSerializer.Serialize(new Dictionary<string, object?> { [name] = null });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonString item = new(node, name);
        string value = item.Value();
        Assert.True(value.Length == 0, "JsonString does not map null to empty");
    }

    /// <summary>
    /// Ensures that JsonString throws when property is missing. Usage example: Assert.Throws(() => new JsonString(node, name).Value()).
    /// </summary>
    [Fact(DisplayName = "JsonString throws when property is missing")]
    public void Given_missing_property_when_read_then_throws()
    {
        string name = $"ключ-{Guid.NewGuid()}-β";
        string miss = $"нет-{Guid.NewGuid()}-η";
        string json = JsonSerializer.Serialize(new Dictionary<string, object> { [name] = $"значение-{Guid.NewGuid()}-σ" });
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement node = document.RootElement;
        JsonString item = new(node, miss);
        Assert.Throws<InvalidOperationException>(() => item.Value());
    }
}
