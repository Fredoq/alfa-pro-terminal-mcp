using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
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
                    long param = RandomNumberGenerator.GetInt32(1, 100_000);
                    long group = RandomNumberGenerator.GetInt32(1, 100_000) * -1;
                    long board = RandomNumberGenerator.GetInt32(1, 100_000);
                    int choice = RandomNumberGenerator.GetInt32(0, 2);
                    long order = choice == 0 ? 1 : 2;
                    long document = 1;
                    long quantity = 1;
                    long price = 1;
                    int slot = RandomNumberGenerator.GetInt32(0, 2);
                    long life = slot == 0 ? 5 : 9;
                    long execution = RandomNumberGenerator.GetInt32(1, 10);
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdAllowedOrderParams = param,
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
                    JsonElement schema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idObjectGroup":{"type":"integer","description":"Object group identifier"},"idMarketBoard":{"type":"integer","description":"Market identifier"},"idOrderType":{"type":"integer","description":"Order type identifier Values: MKT 1 market order, LMT 2 limit order, STP 7 stop market, STL 8 stop limit, TRL 9 trailing limit, TRS 10 trailing stop market, TSL 11 trailing stop limit, RS 12 stop market with take profit, BSL 13 stop limit with take profit, TBRS 28 trailing stop market with take profit"},"idLifeTime":{"type":"integer","description":"Order lifetime identifier Values: 5 end of day, 9 thirty days"}},"required":["idObjectGroup","idMarketBoard","idOrderType","idLifeTime"],"additionalProperties":false}""");
                    McpTool tool = new(new WsAllowedOrderParams(terminal, logger), new Tool { Name = "allowed-order-params", Title = "Allowed order parameters", Description = "Returns allowed order parameter entries.", InputSchema = schema, OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"allowedOrderParams":{"type":"array","description":"Allowed order parameter entries","items":{"type":"object","properties":{"IdAllowedOrderParams":{"type":"integer","description":"Allowed order parameter identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"IdOrderType":{"type":"integer","description":"Order type identifier Values: MKT 1 market order, LMT 2 limit order, STP 7 stop market, STL 8 stop limit, TRL 9 trailing limit, TRS 10 trailing stop market, TSL 11 trailing stop limit, RS 12 stop market with take profit, BSL 13 stop limit with take profit, TBRS 28 trailing stop market with take profit"},"IdDocumentType":{"type":"integer","description":"Document type identifier"},"IdQuantityType":{"type":"integer","description":"Quantity type identifier"},"IdPriceType":{"type":"integer","description":"Price type identifier"},"IdLifeTime":{"type":"integer","description":"Order lifetime identifier"},"IdExecutionType":{"type":"integer","description":"Execution type identifier"}},"required":["IdAllowedOrderParams","IdObjectGroup","IdMarketBoard","IdOrderType","IdDocumentType","IdQuantityType","IdPriceType","IdLifeTime","IdExecutionType"],"additionalProperties":false}}},"required":["allowedOrderParams"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(schema)));
                    Dictionary<string, JsonElement> data = new(StringComparer.Ordinal)
                    {
                        ["idObjectGroup"] = JsonSerializer.SerializeToElement(group),
                        ["idMarketBoard"] = JsonSerializer.SerializeToElement(board),
                        ["idOrderType"] = JsonSerializer.SerializeToElement(order),
                        ["idLifeTime"] = JsonSerializer.SerializeToElement(life)
                    };
                    using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
                    CallToolResult result = await tool.Result(data, source.Token);
                    JsonNode node = result.StructuredContent ?? throw new InvalidOperationException("Structured content is missing");
                    JsonElement output = tool.Tool().OutputSchema ?? throw new InvalidOperationException("Output schema is missing");
                    SchemaMatch probe = new();
                    return probe.Match(node, output);
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Allowed order parameters tool output does not match schema");
    }
}
