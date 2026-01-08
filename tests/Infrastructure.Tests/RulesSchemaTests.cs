using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies rule schema output under concurrency. Usage example: executed by xUnit runner.
/// </summary>
public sealed class RulesSchemaTests
{
    /// <summary>
    /// Ensures that rule schemas return consistent output under concurrency. Usage example: schema.Node(element).
    /// </summary>
    [Fact(DisplayName = "Rules schema returns consistent output under concurrency")]
    public void Rules_schema_returns_consistent_output_under_concurrency()
    {
        long id = RandomNumberGenerator.GetInt32(1_000, 9_999);
        string code = $"code-{Guid.NewGuid():N}-ж";
        string payload = JsonSerializer.Serialize(new { Id = id, Code = code });
        using JsonDocument document = JsonDocument.Parse(payload);
        JsonElement node = document.RootElement;
        RulesSchema schema = new([new WholeRule("Id"), new TextRule("Code")]);
        int count = RandomNumberGenerator.GetInt32(2, 6);
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            ConcurrentBag<string> items = [];
            Parallel.For(0, count, _ => items.Add(schema.Node(node).ToJsonString()));
            string sample = items.First();
            using JsonDocument output = JsonDocument.Parse(sample);
            long value = output.RootElement.GetProperty("Id").GetInt64();
            match = items.All(item => item == sample) && value == id;
        }
        Assert.True(match, "Rules schema does not return consistent output under concurrency");
    }
}
