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
/// Verifies output schema matching for orders tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class OrdersToolTests
{
    /// <summary>
    /// Ensures that orders tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Orders tool returns structured content matching output schema")]
    public async Task Orders_tool_returns_structured_content_matching_output_schema()
    {
        int count = RandomNumberGenerator.GetInt32(2, 5);
        Task<bool>[] tasks = new Task<bool>[count];
        for (int index = 0; index < count; index++)
        {
            tasks[index] = Task.Run(async () =>
            {
                long account = RandomNumberGenerator.GetInt32(10_000, 90_000);
                string note = $"order-{Guid.NewGuid()}-café";
                string comment = $"comment-{Guid.NewGuid()}-naïve";
                string login = $"login-{Guid.NewGuid()}-søren";
                string payload = JsonSerializer.Serialize(new
                {
                    Data = new object[]
                    {
                        new
                        {
                            NumEDocument = (long)RandomNumberGenerator.GetInt32(1, 100_000),
                            ClientOrderNum = RandomNumberGenerator.GetInt32(1, 100_000),
                            IdAccount = account,
                            IdSubAccount = account + 12,
                            IdRazdel = RandomNumberGenerator.GetInt32(1, 1000),
                            IdAllowedOrderParams = RandomNumberGenerator.GetInt32(1, 100_000),
                            AcceptTime = DateTime.UtcNow,
                            IdOrderType = RandomNumberGenerator.GetInt32(1, 3),
                            IdObject = RandomNumberGenerator.GetInt32(1, 100_000),
                            IdMarketBoard = RandomNumberGenerator.GetInt32(1, 1000),
                            LimitPrice = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                            BuySell = RandomNumberGenerator.GetInt32(0, 2) == 0 ? -1 : 1,
                            Quantity = RandomNumberGenerator.GetInt32(1, 10_000),
                            Comment = comment,
                            Login = login,
                            IdOrderStatus = RandomNumberGenerator.GetInt32(1, 30),
                            Rest = RandomNumberGenerator.GetInt32(0, 10_000),
                            Price = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                            BrokerComment = $"broker-{Guid.NewGuid()}-résumé",
                            Note = note
                        }
                    }
                });
                await using BalanceSocketFake terminal = new(payload);
                LoggerFake logger = new();
                McpTool tool = new(new WsOrders(terminal, logger), new Tool { Name = "orders", Title = "Current orders", Description = "Returns current orders for the given account id.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orders":{"type":"array","description":"Current orders for the requested account","items":{"type":"object","properties":{"NumEDocument":{"type":"integer","description":"Order identifier"},"ClientOrderNum":{"type":"integer","description":"Client order number"},"IdAccount":{"type":"integer","description":"Client account id"},"IdSubAccount":{"type":"integer","description":"Client subaccount id"},"IdRazdel":{"type":"integer","description":"Client subaccount portfolio id"},"IdAllowedOrderParams":{"type":"integer","description":"Order parameter combination identifier"},"AcceptTime":{"type":"string","description":"Order acceptance time"},"IdOrderType":{"type":"integer","description":"Order type identifier Values: MKT 1 market order, LMT 2 limit order, STP 7 stop market, STL 8 stop limit, TRL 9 trailing limit, TRS 10 trailing stop market, TSL 11 trailing stop limit, RS 12 stop market with take profit, BSL 13 stop limit with take profit, TBRS 28 trailing stop market with take profit"},"IdObject":{"type":"integer","description":"Security identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"LimitPrice":{"type":"number","description":"Limit order price"},"BuySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"Quantity":{"type":"integer","description":"Quantity in units"},"Comment":{"type":"string","description":"Order comment"},"Login":{"type":"string","description":"Initiator login"},"IdOrderStatus":{"type":"integer","description":"Order status identifier"},"Rest":{"type":"integer","description":"Remaining quantity"},"Price":{"type":"number","description":"Order price"},"BrokerComment":{"type":"string","description":"Broker comment"}},"required":["NumEDocument","ClientOrderNum","IdAccount","IdSubAccount","IdRazdel","IdAllowedOrderParams","AcceptTime","IdOrderType","IdObject","IdMarketBoard","LimitPrice","BuySell","Quantity","Comment","Login","IdOrderStatus","Rest","Price","BrokerComment"],"additionalProperties":false}}},"required":["orders"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}"""))));
                Dictionary<string, JsonElement> data = new() { ["accountId"] = JsonSerializer.SerializeToElement(account) };
                using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
                CallToolResult result = await tool.Result(data, source.Token);
                JsonNode node = result.StructuredContent ?? throw new InvalidOperationException("Structured content is missing");
                JsonElement schema = tool.Tool().OutputSchema ?? throw new InvalidOperationException("Output schema is missing");
                SchemaMatch probe = new();
                return probe.Match(node, schema);
            });
        }
        bool[] list = await Task.WhenAll(tasks);
        bool match = list.All(item => item);
        Assert.True(match, "Orders tool output does not match schema");
    }
}
