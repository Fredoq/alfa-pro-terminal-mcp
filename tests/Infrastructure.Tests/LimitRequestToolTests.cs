using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies output schema matching for limit request tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class LimitRequestToolTests
{
    /// <summary>
    /// Ensures that limit request tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Limit request tool returns structured content matching output schema")]
    public async Task Limit_request_tool_returns_structured_content_matching_output_schema()
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
                    IPAddress host = IPAddress.Parse("127.0.0.1");
                    int port = new Port(host).Value();
                    long account = RandomNumberGenerator.GetInt32(1, 100_000) + port;
                    long razdel = RandomNumberGenerator.GetInt32(1, 100_000);
                    long asset = RandomNumberGenerator.GetInt32(1, 100_000);
                    long board = RandomNumberGenerator.GetInt32(1, 100_000);
                    long document = RandomNumberGenerator.GetInt32(1, 100_000);
                    int side = RandomNumberGenerator.GetInt32(0, 2) == 0 ? -1 : 1;
                    double price = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    int order = RandomNumberGenerator.GetInt32(1, 3);
                    int request = RandomNumberGenerator.GetInt32(3, 5);
                    int quantity = RandomNumberGenerator.GetInt32(1, 1_000_000);
                    int own = RandomNumberGenerator.GetInt32(1, 1_000_000);
                    string note = $"note-{Guid.NewGuid()}-naïve";
                    string payload = JsonSerializer.Serialize(new { Quantity = quantity, QuantityForOwnAssets = own, Note = note });
                    await using LimitSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    LimitRequestTool tool = new(terminal, logger);
                    Dictionary<string, JsonElement> data = new()
                    {
                        ["idAccount"] = JsonSerializer.SerializeToElement(account),
                        ["idRazdel"] = JsonSerializer.SerializeToElement(razdel),
                        ["idObject"] = JsonSerializer.SerializeToElement(asset),
                        ["idMarketBoard"] = JsonSerializer.SerializeToElement(board),
                        ["idDocumentType"] = JsonSerializer.SerializeToElement(document),
                        ["buySell"] = JsonSerializer.SerializeToElement(side),
                        ["price"] = JsonSerializer.SerializeToElement(price),
                        ["idOrderType"] = JsonSerializer.SerializeToElement(order),
                        ["limitRequestType"] = JsonSerializer.SerializeToElement(request)
                    };
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
        Assert.True(match, "Limit request tool output does not match schema");
    }
}
