using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
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
                OrdersTool tool = new(terminal, logger);
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
