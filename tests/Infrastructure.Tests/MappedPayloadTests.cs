using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies MappedPayload serialization. Usage example: executed by xUnit runner.
/// </summary>
public sealed class MappedPayloadTests
{
    /// <summary>
    /// Ensures that MappedPayload serializes input with PascalCase keys. Usage example: new MappedPayload(data, schema).AsString().
    /// </summary>
    [Fact(DisplayName = "MappedPayload serializes input with PascalCase keys")]
    public async Task Mapped_payload_serializes_input_with_pascalcase_keys()
    {
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            Task<bool>[] tasks = new Task<bool>[2];
            for (int index = 0; index < tasks.Length; index++)
            {
                tasks[index] = Task.Run(() =>
                {
                    int value = RandomNumberGenerator.GetInt32(1, 10_000);
                    string text = $"test-{Guid.NewGuid()}-é";
                    InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer"},"note":{"type":"string"}},"required":["idAccount","note"]}"""));
                    Dictionary<string, JsonElement> data = new(StringComparer.Ordinal)
                    {
                        ["idAccount"] = JsonSerializer.SerializeToElement(value),
                        ["note"] = JsonSerializer.SerializeToElement(text)
                    };
                    MappedPayload payload = new(data, schema);
                    string json = payload.AsString();
                    using JsonDocument doc = JsonDocument.Parse(json);
                    bool result = doc.RootElement.TryGetProperty("IdAccount", out JsonElement account) && account.GetInt32() == value && doc.RootElement.TryGetProperty("Note", out JsonElement note) && note.GetString() == text;
                    return result;
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "MappedPayload does not serialize input with PascalCase keys");
    }

    /// <summary>
    /// Ensures that MappedPayload adds extra values with PascalCase keys. Usage example: new MappedPayload(data, schema, extra).AsString().
    /// </summary>
    [Fact(DisplayName = "MappedPayload adds extra values with PascalCase keys")]
    public async Task Mapped_payload_adds_extra_values_with_pascalcase_keys()
    {
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            Task<bool>[] tasks = new Task<bool>[2];
            for (int index = 0; index < tasks.Length; index++)
            {
                tasks[index] = Task.Run(() =>
                {
                    int value = RandomNumberGenerator.GetInt32(1, 10_000);
                    string text = $"note-{Guid.NewGuid()}-é";
                    InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer"},"note":{"type":"string"}},"required":["idAccount","note"],"additionalProperties":false}"""));
                    Dictionary<string, JsonElement> data = new(StringComparer.Ordinal)
                    {
                        ["idAccount"] = JsonSerializer.SerializeToElement(value),
                        ["note"] = JsonSerializer.SerializeToElement(text)
                    };
                    Dictionary<string, JsonElement> extra = new(StringComparer.Ordinal)
                    {
                        ["comment"] = JsonSerializer.SerializeToElement(string.Empty)
                    };
                    MappedPayload payload = new(data, schema, extra);
                    string json = payload.AsString();
                    using JsonDocument doc = JsonDocument.Parse(json);
                    string phrase = doc.RootElement.TryGetProperty("Comment", out JsonElement comment) ? comment.GetString() ?? string.Empty : "missing";
                    bool result = phrase.Length == 0;
                    return result;
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "MappedPayload does not add extra values with PascalCase keys");
    }
}
