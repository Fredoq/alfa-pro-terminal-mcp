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
/// Verifies output schema matching for client subaccounts tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ClientSubAccountsToolTests
{
    /// <summary>
    /// Ensures that client subaccounts tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Client subaccounts tool returns structured content matching output schema")]
    public async Task Client_subaccounts_tool_returns_structured_content_matching_output_schema()
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
                    long account = RandomNumberGenerator.GetInt32(1, 100_000);
                    long subaccount = RandomNumberGenerator.GetInt32(1, 100_000) * -1;
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdSubAccount = subaccount,
                                IdAccount = account
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsClientSubAccounts(terminal, logger), new Tool { Name = "client-subaccounts", Title = "Client subaccounts", Description = "Returns client subaccount entries.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"clientSubAccounts":{"type":"array","description":"Client subaccount entries","items":{"type":"object","properties":{"IdSubAccount":{"type":"integer","description":"Client subaccount identifier"},"IdAccount":{"type":"integer","description":"Client account identifier"}},"required":["IdSubAccount","IdAccount"],"additionalProperties":false}}},"required":["clientSubAccounts"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new FixedPayloadPlan(new EmptyInputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""")), new EntityPayload("ClientSubAccountEntity", true)));
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
        Assert.True(match, "Client subaccounts tool output does not match schema");
    }
}
