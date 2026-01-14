using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for allowed order parameters tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AllowedOrderParamsToolTests
{
    /// <summary>
    /// Ensures that allowed order parameters tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Allowed order parameters tool returns structured content matching output schema")]
    public async Task Allowed_order_parameters_tool_returns_structured_content_matching_output_schema()
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
                    long allowed = RandomNumberGenerator.GetInt32(1, 100_000);
                    long group = RandomNumberGenerator.GetInt32(1, 100_000) * -1;
                    long board = RandomNumberGenerator.GetInt32(1, 100_000);
                    long order = RandomNumberGenerator.GetInt32(1, 5);
                    long document = RandomNumberGenerator.GetInt32(1, 5);
                    long quantity = RandomNumberGenerator.GetInt32(1, 5);
                    long price = RandomNumberGenerator.GetInt32(1, 5);
                    long life = RandomNumberGenerator.GetInt32(1, 20) * -1;
                    long execution = RandomNumberGenerator.GetInt32(1, 10);
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdAllowedOrderParams = allowed,
                                IdObjectGroup = group,
                                IdMarketBoard = board,
                                IdOrderType = order,
                                IdDocumentType = document,
                                IdQuantityType = quantity,
                                IdPriceType = price,
                                IdLifeTime = life,
                                IdExecutionType = execution
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsAllowedOrderParams(terminal, logger), new Tool { Name = "allowed-order-params", Title = "Allowed order parameters", Description = "Returns allowed order parameter entries.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"allowedOrderParams":{"type":"array","description":"Allowed order parameter entries","items":{"type":"object","properties":{"IdAllowedOrderParams":{"type":"integer","description":"Allowed order parameter identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"IdOrderType":{"type":"integer","description":"Order type identifier"},"IdDocumentType":{"type":"integer","description":"Document type identifier"},"IdQuantityType":{"type":"integer","description":"Quantity type identifier"},"IdPriceType":{"type":"integer","description":"Price type identifier"},"IdLifeTime":{"type":"integer","description":"Order lifetime identifier"},"IdExecutionType":{"type":"integer","description":"Execution type identifier"}},"required":["IdAllowedOrderParams","IdObjectGroup","IdMarketBoard","IdOrderType","IdDocumentType","IdQuantityType","IdPriceType","IdLifeTime","IdExecutionType"],"additionalProperties":false}}},"required":["allowedOrderParams"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new FixedPayloadPlan(new EmptyInputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""")), new AllowedOrderParamEntity()));
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
        Assert.True(match, "Allowed order parameters tool output does not match schema");
    }
}
