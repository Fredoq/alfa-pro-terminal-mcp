using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies value rule output under concurrency. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ValueRuleTests
{
    /// <summary>
    /// Ensures that value rules return consistent output under concurrency. Usage example: rule.Apply(item, output).
    /// </summary>
    [Fact(DisplayName = "Value rule returns consistent output under concurrency")]
    public void Value_rule_returns_consistent_output_under_concurrency()
    {
        long id = RandomNumberGenerator.GetInt32(1_000, 9_999);
        string note = $"note-{Guid.NewGuid():N}-μ";
        string payload = JsonSerializer.Serialize(new { Id = id, Note = note });
        JsonNode data = JsonNode.Parse(payload) ?? throw new InvalidOperationException("Payload is missing");
        JsonObject node = data.AsObject();
        ValueRule<long> rule = new(new JsonInteger(node, "Id"), "Id");
        int count = RandomNumberGenerator.GetInt32(2, 6);
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            ConcurrentBag<string> items = [];
            Parallel.For(0, count, _ =>
            {
                JsonObject root = [];
                rule.Apply(node, root);
                items.Add(root.ToJsonString());
            });
            string sample = items.First();
            using JsonDocument output = JsonDocument.Parse(sample);
            long value = output.RootElement.GetProperty("Id").GetInt64();
            match = items.All(item => item == sample) && value == id;
        }
        Assert.True(match, "Value rule does not return consistent output under concurrency");
    }
}
