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
                    long group = RandomNumberGenerator.GetInt32(1, 100_000);
                    long market = RandomNumberGenerator.GetInt32(1, 100_000);
                    double limit = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    double trigger = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    double alternative = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    int side = RandomNumberGenerator.GetInt32(0, 2) == 0 ? -1 : 1;
                    int quantity = RandomNumberGenerator.GetInt32(1, 10_000);
                    string comment = $"comment-{Guid.NewGuid()}-naïve";
                    string code = $"rcode-{Guid.NewGuid()}-søren";
                    long allowed = RandomNumberGenerator.GetInt32(1, 100_000);
                    int status = RandomNumberGenerator.GetInt32(0, 2);
                    int error = RandomNumberGenerator.GetInt32(0, 10);
                    string assets = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdObject = asset,
                                Ticker = $"TCK{RandomNumberGenerator.GetInt32(1, 1000)}",
                                ISIN = $"ISIN-{Guid.NewGuid()}",
                                Name = $"name-{Guid.NewGuid()}-café",
                                Description = $"desc-{Guid.NewGuid()}-naïve",
                                Nominal = RandomNumberGenerator.GetInt32(1, 100_000) / 10d,
                                IdObjectType = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdObjectGroup = group,
                                IdObjectBase = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdObjectFaceUnit = RandomNumberGenerator.GetInt32(1, 100_000),
                                MatDateObject = DateTime.UtcNow.ToString("O"),
                                Instruments = new object[]
                                {
                                    new
                                    {
                                        IdFi = RandomNumberGenerator.GetInt32(1, 100_000),
                                        RCode = code,
                                        IsLiquid = true,
                                        IdMarketBoard = market
                                    }
                                }
                            }
                        }
                    });
                    string subs = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new { IdSubAccount = subaccount, IdAccount = account },
                            new { IdSubAccount = subaccount + 1, IdAccount = account + 1 }
                        }
                    });
                    string razdels = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new { IdRazdel = razdel, IdAccount = account, IdSubAccount = subaccount, IdRazdelGroup = RandomNumberGenerator.GetInt32(1, 100_000), RCode = code },
                            new { IdRazdel = razdel + 1, IdAccount = account + 2, IdSubAccount = subaccount + 2, IdRazdelGroup = RandomNumberGenerator.GetInt32(1, 100_000), RCode = $"rcode-{Guid.NewGuid()}-café" }
                        }
                    });
                    string combos = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new { IdAllowedOrderParams = allowed, IdObjectGroup = group, IdMarketBoard = market, IdOrderType = 2, IdDocumentType = 1, IdQuantityType = 1, IdPriceType = 1, IdLifeTime = 9, IdExecutionType = 1 },
                            new { IdAllowedOrderParams = allowed + 1, IdObjectGroup = group, IdMarketBoard = market, IdOrderType = 1, IdDocumentType = 1, IdQuantityType = 1, IdPriceType = 1, IdLifeTime = 9, IdExecutionType = 1 }
                        }
                    });
                    string payload = JsonSerializer.Serialize(new { Status = status, Message = (string?)null, Error = (object?)null, Value = new { ClientOrderNum = RandomNumberGenerator.GetInt32(1, 100_000), NumEDocument = (long)RandomNumberGenerator.GetInt32(1, 100_000), ErrorCode = error, ErrorText = (string?)null }, Extra = "ignored" });
                    Dictionary<string, string> responses = new(StringComparer.Ordinal)
                    {
                        ["AssetInfoEntity"] = assets,
                        ["ClientSubAccountEntity"] = subs,
                        ["SubAccountRazdelEntity"] = razdels,
                        ["AllowedOrderParamEntity"] = combos,
                        ["#Order.Enter.Query"] = payload
                    };
                    await using OrderEntryFlowSocketFake terminal = new(responses);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsOrderEntry(terminal, logger), new Tool { Name = "order-enter", Title = "Order entry", Description = "Places a new order and returns the broker response.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier Values: 1 index value, 2 exchange indicative settlement price, 3 trade price during the session excluding open and close trades, 4 trade price during the main session excluding additional sessions and open and close trades, 5 trade yield during the session excluding open and close trades, 6 trade yield during the main session excluding additional sessions and open and close trades, 7 bid price during the session excluding open and close orders, 8 bid price during the main session excluding additional sessions and open and close orders, 9 bid yield during the session excluding open and close, 10 bid price during the main session excluding additional sessions and open and close, 11 ask price during the session excluding open and close, 12 ask price during the main session excluding additional sessions and open and close, 13 ask yield during the session excluding open and close, 14 ask price during the main session excluding additional sessions and open and close"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"}},"required":["idAccount","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment"],"additionalProperties":false}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orderEntry":{"type":"object","description":"Order entry response","properties":{"ResponseStatus":{"type":"integer","description":"Response status: 0 for OK, otherwise error"},"Message":{"type":"string","description":"Response status message"},"Error":{"type":"object","description":"Response error details","properties":{"Code":{"type":"integer","description":"Error code"},"Message":{"type":"string","description":"Error message"}},"required":["Code","Message"],"additionalProperties":false},"Value":{"type":"object","description":"Order entry response data","properties":{"ClientOrderNum":{"type":"integer","description":"Client order number"},"NumEDocument":{"type":"integer","description":"Broker order identifier"},"ErrorCode":{"type":"integer","description":"Terminal error code"},"ErrorText":{"type":"string","description":"Terminal error text"}},"required":["ClientOrderNum","NumEDocument","ErrorCode","ErrorText"],"additionalProperties":false}},"required":["ResponseStatus","Message","Error","Value"],"additionalProperties":false}},"required":["orderEntry"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = false, IdempotentHint = false, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier Values: 1 index value, 2 exchange indicative settlement price, 3 trade price during the session excluding open and close trades, 4 trade price during the main session excluding additional sessions and open and close trades, 5 trade yield during the session excluding open and close trades, 6 trade yield during the main session excluding additional sessions and open and close trades, 7 bid price during the session excluding open and close orders, 8 bid price during the main session excluding additional sessions and open and close orders, 9 bid yield during the session excluding open and close, 10 bid price during the main session excluding additional sessions and open and close, 11 ask price during the session excluding open and close, 12 ask price during the main session excluding additional sessions and open and close, 13 ask yield during the session excluding open and close, 14 ask price during the main session excluding additional sessions and open and close"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"}},"required":["idAccount","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment"],"additionalProperties":false}"""))));
                    Dictionary<string, JsonElement> data = new()
                    {
                        ["idAccount"] = JsonSerializer.SerializeToElement(account),
                        ["idPriceControlType"] = JsonSerializer.SerializeToElement(control),
                        ["idObject"] = JsonSerializer.SerializeToElement(asset),
                        ["limitPrice"] = JsonSerializer.SerializeToElement(limit),
                        ["stopPrice"] = JsonSerializer.SerializeToElement(trigger),
                        ["limitLevelAlternative"] = JsonSerializer.SerializeToElement(alternative),
                        ["buySell"] = JsonSerializer.SerializeToElement(side),
                        ["quantity"] = JsonSerializer.SerializeToElement(quantity),
                        ["comment"] = JsonSerializer.SerializeToElement(comment)
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

    /// <summary>
    /// Ensures that order entry tool derives missing identifiers before sending payload. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Order entry tool derives payload identifiers")]
    public async Task Order_entry_tool_derives_payload_identifiers()
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
                    long group = RandomNumberGenerator.GetInt32(1, 100_000);
                    long market = RandomNumberGenerator.GetInt32(1, 100_000);
                    double limit = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    double trigger = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    double alternative = RandomNumberGenerator.GetInt32(1, 100_000) / 10d;
                    int side = RandomNumberGenerator.GetInt32(0, 2) == 0 ? -1 : 1;
                    int quantity = RandomNumberGenerator.GetInt32(1, 10_000);
                    string comment = $"comment-{Guid.NewGuid()}-naïve";
                    string code = $"rcode-{Guid.NewGuid()}-søren";
                    long allowed = RandomNumberGenerator.GetInt32(1, 100_000);
                    string assets = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdObject = asset,
                                Ticker = $"TCK{RandomNumberGenerator.GetInt32(1, 1000)}",
                                ISIN = $"ISIN-{Guid.NewGuid()}",
                                Name = $"name-{Guid.NewGuid()}-café",
                                Description = $"desc-{Guid.NewGuid()}-naïve",
                                Nominal = RandomNumberGenerator.GetInt32(1, 100_000) / 10d,
                                IdObjectType = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdObjectGroup = group,
                                IdObjectBase = RandomNumberGenerator.GetInt32(1, 100_000),
                                IdObjectFaceUnit = RandomNumberGenerator.GetInt32(1, 100_000),
                                MatDateObject = DateTime.UtcNow.ToString("O"),
                                Instruments = new object[]
                                {
                                    new
                                    {
                                        IdFi = RandomNumberGenerator.GetInt32(1, 100_000),
                                        RCode = code,
                                        IsLiquid = true,
                                        IdMarketBoard = market
                                    }
                                }
                            }
                        }
                    });
                    string subs = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new { IdSubAccount = subaccount, IdAccount = account },
                            new { IdSubAccount = subaccount + 1, IdAccount = account + 1 }
                        }
                    });
                    string razdels = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new { IdRazdel = razdel, IdAccount = account, IdSubAccount = subaccount, IdRazdelGroup = RandomNumberGenerator.GetInt32(1, 100_000), RCode = code },
                            new { IdRazdel = razdel + 1, IdAccount = account + 2, IdSubAccount = subaccount + 2, IdRazdelGroup = RandomNumberGenerator.GetInt32(1, 100_000), RCode = $"rcode-{Guid.NewGuid()}-café" }
                        }
                    });
                    string combos = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new { IdAllowedOrderParams = allowed, IdObjectGroup = group, IdMarketBoard = market, IdOrderType = 2, IdDocumentType = 1, IdQuantityType = 1, IdPriceType = 1, IdLifeTime = 9, IdExecutionType = 1 },
                            new { IdAllowedOrderParams = allowed + 1, IdObjectGroup = group, IdMarketBoard = market, IdOrderType = 1, IdDocumentType = 1, IdQuantityType = 1, IdPriceType = 1, IdLifeTime = 9, IdExecutionType = 1 }
                        }
                    });
                    string entry = JsonSerializer.Serialize(new { Status = 0, Message = (string?)null, Error = (object?)null, Value = new { ClientOrderNum = RandomNumberGenerator.GetInt32(1, 100_000), NumEDocument = (long)RandomNumberGenerator.GetInt32(1, 100_000), ErrorCode = 0, ErrorText = (string?)null }, Extra = "ignored" });
                    Dictionary<string, string> responses = new(StringComparer.Ordinal)
                    {
                        ["AssetInfoEntity"] = assets,
                        ["ClientSubAccountEntity"] = subs,
                        ["SubAccountRazdelEntity"] = razdels,
                        ["AllowedOrderParamEntity"] = combos,
                        ["#Order.Enter.Query"] = entry
                    };
                    await using OrderEntryFlowSocketFake terminal = new(responses);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsOrderEntry(terminal, logger), new Tool { Name = "order-enter", Title = "Order entry", Description = "Places a new order and returns the broker response.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier Values: 1 index value, 2 exchange indicative settlement price, 3 trade price during the session excluding open and close trades, 4 trade price during the main session excluding additional sessions and open and close trades, 5 trade yield during the session excluding open and close trades, 6 trade yield during the main session excluding additional sessions and open and close trades, 7 bid price during the session excluding open and close orders, 8 bid price during the main session excluding additional sessions and open and close orders, 9 bid yield during the session excluding open and close, 10 bid price during the main session excluding additional sessions and open and close, 11 ask price during the session excluding open and close, 12 ask price during the main session excluding additional sessions and open and close, 13 ask yield during the session excluding open and close, 14 ask price during the main session excluding additional sessions and open and close"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"}},"required":["idAccount","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment"],"additionalProperties":false}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"orderEntry":{"type":"object","description":"Order entry response","properties":{"ResponseStatus":{"type":"integer","description":"Response status: 0 for OK, otherwise error"},"Message":{"type":"string","description":"Response status message"},"Error":{"type":"object","description":"Response error details","properties":{"Code":{"type":"integer","description":"Error code"},"Message":{"type":"string","description":"Error message"}},"required":["Code","Message"],"additionalProperties":false},"Value":{"type":"object","description":"Order entry response data","properties":{"ClientOrderNum":{"type":"integer","description":"Client order number"},"NumEDocument":{"type":"integer","description":"Broker order identifier"},"ErrorCode":{"type":"integer","description":"Terminal error code"},"ErrorText":{"type":"string","description":"Terminal error text"}},"required":["ClientOrderNum","NumEDocument","ErrorCode","ErrorText"],"additionalProperties":false}},"required":["ResponseStatus","Message","Error","Value"],"additionalProperties":false}},"required":["orderEntry"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = false, IdempotentHint = false, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idPriceControlType":{"type":"integer","description":"Price control type identifier Values: 1 index value, 2 exchange indicative settlement price, 3 trade price during the session excluding open and close trades, 4 trade price during the main session excluding additional sessions and open and close trades, 5 trade yield during the session excluding open and close trades, 6 trade yield during the main session excluding additional sessions and open and close trades, 7 bid price during the session excluding open and close orders, 8 bid price during the main session excluding additional sessions and open and close orders, 9 bid yield during the session excluding open and close, 10 bid price during the main session excluding additional sessions and open and close, 11 ask price during the session excluding open and close, 12 ask price during the main session excluding additional sessions and open and close, 13 ask yield during the session excluding open and close, 14 ask price during the main session excluding additional sessions and open and close"},"idObject":{"type":"integer","description":"Security identifier"},"limitPrice":{"type":"number","description":"Limit price"},"stopPrice":{"type":"number","description":"Stop price"},"limitLevelAlternative":{"type":"number","description":"Alternative limit price"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"quantity":{"type":"integer","description":"Quantity in units"},"comment":{"type":"string","description":"Order comment"}},"required":["idAccount","idPriceControlType","idObject","limitPrice","stopPrice","limitLevelAlternative","buySell","quantity","comment"],"additionalProperties":false}"""))));
                    Dictionary<string, JsonElement> data = new()
                    {
                        ["idAccount"] = JsonSerializer.SerializeToElement(account),
                        ["idPriceControlType"] = JsonSerializer.SerializeToElement(control),
                        ["idObject"] = JsonSerializer.SerializeToElement(asset),
                        ["limitPrice"] = JsonSerializer.SerializeToElement(limit),
                        ["stopPrice"] = JsonSerializer.SerializeToElement(trigger),
                        ["limitLevelAlternative"] = JsonSerializer.SerializeToElement(alternative),
                        ["buySell"] = JsonSerializer.SerializeToElement(side),
                        ["quantity"] = JsonSerializer.SerializeToElement(quantity),
                        ["comment"] = JsonSerializer.SerializeToElement(comment)
                    };
                    using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
                    await tool.Result(data, source.Token);
                    string request = string.Empty;
                    foreach (string item in terminal.Items())
                    {
                        using JsonDocument node = JsonDocument.Parse(item);
                        string channel = node.RootElement.GetProperty("Channel").GetString() ?? string.Empty;
                        if (channel != "#Order.Enter.Query")
                        {
                            continue;
                        }
                        request = item;
                        break;
                    }
                    if (request.Length == 0)
                    {
                        throw new InvalidOperationException("Order entry request is missing");
                    }
                    using JsonDocument packet = JsonDocument.Parse(request);
                    string payload = packet.RootElement.GetProperty("Payload").GetString() ?? string.Empty;
                    using JsonDocument body = JsonDocument.Parse(payload);
                    JsonElement root = body.RootElement;
                    bool flag = root.GetProperty("IdSubAccount").GetInt64() == subaccount && root.GetProperty("IdRazdel").GetInt64() == razdel && root.GetProperty("IdAllowedOrderParams").GetInt64() == allowed;
                    return flag;
                });
            }
            bool[] list = await Task.WhenAll(tasks);
            match = list.All(item => item);
        }
        Assert.True(match, "Order entry tool payload does not use derived identifiers");
    }
}
