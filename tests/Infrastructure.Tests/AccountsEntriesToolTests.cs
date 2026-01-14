using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for accounts entries tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AccountsEntriesToolTests
{
    /// <summary>
    /// Ensures that accounts entries tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Accounts entries tool returns structured content matching output schema")]
    public async Task Accounts_entries_tool_returns_structured_content_matching_output_schema()
    {
        int count = RandomNumberGenerator.GetInt32(2, 5);
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            Task<bool>[] tasks = new Task<bool>[count];
            for (int index = 0; index < count; index++)
            {
                tasks[index] = Task.Run(async () =>
                {
                    long account = RandomNumberGenerator.GetInt32(10_000, 90_000);
                    int code = RandomNumberGenerator.GetInt32(0, 3);
                    string note = $"тест-{Guid.NewGuid()}-λ";
                    string payload = JsonSerializer.Serialize(new { Data = new object[] { new { IdAccount = account, IIAType = code, Note = note } } });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    AccountsEntriesPlan plan = new();
                    McpTool tool = new(new WsAccounts(terminal, logger), plan);
                    Dictionary<string, JsonElement> data = new();
                    using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
                    CallToolResult result = await tool.Result(data, source.Token);
                    JsonNode node = result.StructuredContent ?? throw new InvalidOperationException("Structured content is missing");
                    JsonElement schema = tool.Tool().OutputSchema ?? throw new InvalidOperationException("Output schema is missing");
                    SchemaMatch probe = new();
                    return probe.Match(node, schema);
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Accounts entries tool output does not match schema");
    }
}
