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
/// Verifies output schema matching for subaccount portfolios tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class SubAccountRazdelsToolTests
{
    /// <summary>
    /// Ensures that subaccount portfolios tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Subaccount portfolios tool returns structured content matching output schema")]
    public async Task Subaccount_portfolios_tool_returns_structured_content_matching_output_schema()
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
                    long razdel = RandomNumberGenerator.GetInt32(1, 100_000);
                    long group = RandomNumberGenerator.GetInt32(1, 100_000) * -1;
                    string code = $"R-{Guid.NewGuid():N}-~";
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdRazdel = razdel,
                                IdAccount = account,
                                IdSubAccount = subaccount,
                                IdRazdelGroup = group,
                                RCode = code
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsSubAccountRazdels(terminal, logger), new Tool { Name = "subaccount-razdels", Title = "Subaccount portfolios", Description = "Returns subaccount portfolio entries.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"subAccountRazdels":{"type":"array","description":"Subaccount portfolio entries","items":{"type":"object","properties":{"IdRazdel":{"type":"integer","description":"Portfolio identifier"},"IdAccount":{"type":"integer","description":"Client account identifier"},"IdSubAccount":{"type":"integer","description":"Client subaccount identifier"},"IdRazdelGroup":{"type":"integer","description":"Portfolio group identifier"},"RCode":{"type":"string","description":"Portfolio code"}},"required":["IdRazdel","IdAccount","IdSubAccount","IdRazdelGroup","RCode"],"additionalProperties":false}}},"required":["subAccountRazdels"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new FixedPayloadPlan(new EmptyInputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""")), new EntityPayload("SubAccountRazdelEntity", true)));
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
        Assert.True(match, "Subaccount portfolios tool output does not match schema");
    }
}
