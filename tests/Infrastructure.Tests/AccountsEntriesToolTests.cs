using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
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
                    McpTool tool = new(new WsAccounts(terminal, logger), new Tool { Name = "entries", Title = "Accounts entries", Description = "Returns a collection of client accounts. Each account contains an identifier and IIA type.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accounts":{"type":"array","description":"List of brokerage accounts available to the user","items":{"type":"object","properties":{"AccountId":{"type":"integer","description":"Unique identifier of the brokerage account used to reference the account in subsequent operations"},"IIAType":{"type":"integer","enum":[0,1,2],"description":"Individual Investment Account type code Values: 0 standard account, 1 IIA Type A, 2 IIA Type B"}},"required":["AccountId","IIAType"],"additionalProperties":false}}},"required":["accounts"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new FixedPayloadPlan(new EmptyInputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""")), new EntityPayload("ClientAccountEntity", true)));
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
