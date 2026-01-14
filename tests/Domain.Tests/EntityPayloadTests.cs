using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

/// <summary>
/// Verifies EntityPayload serialization. Usage example: executed by xUnit runner.
/// </summary>
public sealed class EntityPayloadTests
{
    /// <summary>
    /// Ensures EntityPayload serializes type and init values. Usage example: new EntityPayload(type, init).AsString().
    /// </summary>
    [Fact(DisplayName = "Entity payload serializes type and init values")]
    public async Task Entity_payload_serializes_type_and_init_values()
    {
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            int count = RandomNumberGenerator.GetInt32(2, 5);
            Task<bool>[] tasks = new Task<bool>[count];
            for (int index = 0; index < count; index++)
            {
                tasks[index] = Task.Run(() =>
                {
                    string type = $"тип-{Guid.NewGuid()}-ß";
                    bool init = RandomNumberGenerator.GetInt32(0, 2) == 0;
                    EntityPayload payload = new(type, init);
                    string json = payload.AsString();
                    using JsonDocument document = JsonDocument.Parse(json);
                    JsonElement root = document.RootElement;
                    int size = 0;
                    bool flag = false;
                    bool mark = false;
                    foreach (JsonProperty item in root.EnumerateObject())
                    {
                        size++;
                        if (item.Value.ValueKind == JsonValueKind.String && item.Value.GetString() == type)
                        {
                            flag = true;
                        }
                        if ((item.Value.ValueKind == JsonValueKind.True || item.Value.ValueKind == JsonValueKind.False) && item.Value.GetBoolean() == init)
                        {
                            mark = true;
                        }
                    }
                    return size == 2 && flag && mark;
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Entity payload does not serialize type and init values");
    }
}
