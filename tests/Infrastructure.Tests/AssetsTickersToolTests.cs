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
/// Verifies output schema matching for asset info by tickers tool. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AssetsTickersToolTests
{
    /// <summary>
    /// Ensures that asset info by tickers tool output matches declared schema. Usage example: await tool.Result(data, token).
    /// </summary>
    [Fact(DisplayName = "Asset info by tickers tool returns structured content matching output schema")]
    public async Task Asset_info_by_tickers_tool_returns_structured_content_matching_output_schema()
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
                    long id = RandomNumberGenerator.GetInt32(10_000, 90_000);
                    string ticker = $"ТКР{id}";
                    string name = $"название-{Guid.NewGuid()}-π";
                    string text = $"описание-{Guid.NewGuid()}-θ";
                    string date = DateTime.UtcNow.AddDays(-RandomNumberGenerator.GetInt32(1, 10)).ToString("O");
                    string payload = JsonSerializer.Serialize(new
                    {
                        Data = new object[]
                        {
                            new
                            {
                                IdObject = id,
                                Ticker = ticker,
                                ISIN = $"ISIN{id}",
                                Name = name,
                                Description = text,
                                Nominal = RandomNumberGenerator.GetInt32(1, 1000) / 10d,
                                IdObjectType = RandomNumberGenerator.GetInt32(1, 100),
                                IdObjectGroup = RandomNumberGenerator.GetInt32(1, 100),
                                IdObjectBase = RandomNumberGenerator.GetInt32(1, 100),
                                IdObjectFaceUnit = RandomNumberGenerator.GetInt32(1, 100),
                                MatDateObject = date,
                                Instruments = new object[]
                                {
                                    new
                                    {
                                        IdFi = RandomNumberGenerator.GetInt32(1, 100_000),
                                        RCode = $"код-{Guid.NewGuid()}-ж",
                                        IsLiquid = true,
                                        IdMarketBoard = RandomNumberGenerator.GetInt32(1, 100)
                                    }
                                }
                            }
                        }
                    });
                    await using BalanceSocketFake terminal = new(payload);
                    LoggerFake logger = new();
                    McpTool tool = new(new WsAssetsInfo(terminal, logger), new Tool { Name = "infoByTickers", Title = "Asset info by tickers", Description = "Returns asset info list for the given ticker symbols.", InputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"tickers":{"type":"array","description":"Collection of ticker symbols to extract","items":{"type":"string"}}},"required":["tickers"]}"""), OutputSchema = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"assets":{"type":"array","description":"Asset info entries for requested tickers","items":{"type":"object","properties":{"IdObject":{"type":"integer","description":"Asset identifier"},"Ticker":{"type":"string","description":"Exchange ticker"},"ISIN":{"type":"string","description":"International security identifier"},"Name":{"type":"string","description":"Asset name"},"Description":{"type":"string","description":"Asset description"},"Nominal":{"type":"number","description":"Nominal value"},"IdObjectType":{"type":"integer","description":"Asset type identifier"},"IdObjectGroup":{"type":"integer","description":"Asset group identifier"},"IdObjectBase":{"type":"integer","description":"Base asset identifier"},"IdObjectFaceUnit":{"type":"integer","description":"Face value currency identifier"},"MatDateObject":{"type":"string","description":"Expiration date of asset"},"Instruments":{"type":"array","description":"Trading instrument details","items":{"type":"object","properties":{"IdFi":{"type":"integer","description":"Financial instrument identifier"},"RCode":{"type":"string","description":"Portfolio code"},"IsLiquid":{"type":"boolean","description":"Liquidity flag"},"IdMarketBoard":{"type":"integer","description":"Market identifier"}},"required":["IdFi","RCode","IsLiquid","IdMarketBoard"],"additionalProperties":false}}},"required":["IdObject","Ticker","ISIN","Name","Description","Nominal","IdObjectType","IdObjectGroup","IdObjectBase","IdObjectFaceUnit","MatDateObject","Instruments"],"additionalProperties":false}}},"required":["assets"],"additionalProperties":false}"""), Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } }, new MappedPayloadPlan(new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"tickers":{"type":"array","description":"Collection of ticker symbols to extract","items":{"type":"string"}}},"required":["tickers"]}"""))));
                    Dictionary<string, JsonElement> data = new() { ["tickers"] = JsonSerializer.SerializeToElement(new[] { ticker }) };
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
        Assert.True(match, "Asset info by tickers tool output does not match schema");
    }
}
