using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies EmptyInputSchema behavior. Usage example: executed by xUnit runner.
/// </summary>
public sealed class EmptyInputSchemaTests
{
    /// <summary>
    /// Ensures that EmptyInputSchema accepts empty arguments. Usage example: schema.Ensure(data).
    /// </summary>
    [Fact(DisplayName = "Empty input schema accepts empty arguments")]
    public async Task Empty_input_schema_accepts_empty_arguments()
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
                    EmptyInputSchema schema = new EmptyInputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
                    Dictionary<string, JsonElement> data = new(StringComparer.Ordinal);
                    schema.Ensure(data);
                    return data.Count == 0;
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Empty input schema does not accept empty arguments");
    }

    /// <summary>
    /// Ensures that EmptyInputSchema rejects non-empty arguments. Usage example: schema.Ensure(data).
    /// </summary>
    [Fact(DisplayName = "Empty input schema rejects unexpected arguments")]
    public async Task Empty_input_schema_rejects_unexpected_arguments()
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
                    int value = RandomNumberGenerator.GetInt32(1, 100_000);
                    string key = $"ключ-{Guid.NewGuid()}-π";
                    EmptyInputSchema schema = new EmptyInputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
                    Dictionary<string, JsonElement> data = new(StringComparer.Ordinal) { [key] = JsonSerializer.SerializeToElement(value) };
                    try
                    {
                        schema.Ensure(data);
                        return false;
                    }
                    catch (McpProtocolException)
                    {
                        return true;
                    }
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Empty input schema does not reject unexpected arguments");
    }
}
