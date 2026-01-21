using System.Net;
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
/// Verifies output schema matching for order entry tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class OrderEntryToolTests
{
    /// <summary>
    /// Ensures that order entry tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Order entry tool returns structured content matching output schema")]
    public async Task Order_entry_tool_returns_structured_content_matching_output_schema()
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
                    long subaccount = account + RandomNumberGenerator.GetInt32(1, 10_000);
                    long razdel = RandomNumberGenerator.GetInt32(1, 100_000);
                    int control = RandomNumberGenerator.GetInt32(1, 15);
                    long asset = RandomNumberGenerator.GetInt32(1, 100_000);
                    double limit = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    double trigger = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    double alternative = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    int side = RandomNumberGenerator.GetInt32(0, 2) == 0 ? -1 : 1;
                    int quantity = RandomNumberGenerator.GetInt32(1, 10_000);
                    string comment = $"comment-{Guid.NewGuid()}-naïve";
                    long allowed = RandomNumberGenerator.GetInt32(1, 100_000);
                    int status = RandomNumberGenerator.GetInt32(0, 2);
                    int error = RandomNumberGenerator.GetInt32(0, 10);
                    string payload = JsonSerializer.Serialize(new { Status = status, Message = (string?)null, Error = (object?)null, Value = new { ClientOrderNum = RandomNumberGenerator.GetInt32(1, 100_000), NumEDocument = (long)RandomNumberGenerator.GetInt32(1, 100_000), ErrorCode = error, ErrorText = (string?)null }, Extra = "ignored" });
                    await using OrderEntrySocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsOrderEntry(terminal, logger), new Tool { Name = "order-enter", Title = "Order entry", Description = "Places a new order and returns the broker response.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idSubAccount":{"type":"integer","description":"Client subaccount identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"},"idAllowedOrderParams":{"type":"integer","description":"Allowed order parameters identifier"}},"required":["idAccount","idSubAccount","idRazdel","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment","idAllowedOrderParams"],"additionalProperties":false}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orderEntry":{"type":"object","description":"Order entry response","properties":{"ResponseStatus":{"type":"integer","description":"Response status: 0 for OK, otherwise error"},"Message":{"type":"string","description":"Response status message"},"Error":{"type":"object","description":"Response error details","properties":{"Code":{"type":"integer","description":"Error code"},"Message":{"type":"string","description":"Error message"}},"required":["Code","Message"],"additionalProperties":false},"Value":{"type":"object","description":"Order entry response data","properties":{"ClientOrderNum":{"type":"integer","description":"Client order number"},"NumEDocument":{"type":"integer","description":"Broker order identifier"},"ErrorCode":{"type":"integer","description":"Terminal error code"},"ErrorText":{"type":"string","description":"Terminal error text"}},"required":["ClientOrderNum","NumEDocument","ErrorCode","ErrorText"],"additionalProperties":false}},"required":["ResponseStatus","Message","Error","Value"],"additionalProperties":false}},"required":["orderEntry"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = false, IdempotentHint = false, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idSubAccount":{"type":"integer","description":"Client subaccount identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"},"idAllowedOrderParams":{"type":"integer","description":"Allowed order parameters identifier"}},"required":["idAccount","idSubAccount","idRazdel","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment","idAllowedOrderParams"],"additionalProperties":false}"""))));
                    Dictionary<string, JsonElement> data = new()
                    {
                        ["idAccount"] = JsonSerializer.SerializeToElement(account),
                        ["idSubAccount"] = JsonSerializer.SerializeToElement(subaccount),
                        ["idRazdel"] = JsonSerializer.SerializeToElement(razdel),
                        ["idPriceControlType"] = JsonSerializer.SerializeToElement(control),
                        ["idObject"] = JsonSerializer.SerializeToElement(asset),
                        ["limitPrice"] = JsonSerializer.SerializeToElement(limit),
                        ["stopPrice"] = JsonSerializer.SerializeToElement(trigger),
                        ["limitLevelAlternative"] = JsonSerializer.SerializeToElement(alternative),
                        ["buySell"] = JsonSerializer.SerializeToElement(side),
                        ["quantity"] = JsonSerializer.SerializeToElement(quantity),
                        ["comment"] = JsonSerializer.SerializeToElement(comment),
                        ["idAllowedOrderParams"] = JsonSerializer.SerializeToElement(allowed)
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
        Assert.True(match, "Order entry tool output does not match schema");
    }
}
