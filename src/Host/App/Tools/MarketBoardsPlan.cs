using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for market board entries. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class MarketBoardsPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"marketBoards":{"type":"array","description":"Market board dictionary entries","items":{"type":"object","properties":{"IdMarketBoard":{"type":"integer","description":"Market board identifier"},"NameMarketBoard":{"type":"string","description":"Market board name"},"DescMarketBoard":{"type":"string","description":"Market board description"},"RCode":{"type":"string","description":"Portfolio code traded on the market board"},"IdObjectCurrency":{"type":"integer","description":"Currency object identifier for the market board"}},"required":["IdMarketBoard","NameMarketBoard","DescMarketBoard","RCode","IdObjectCurrency"],"additionalProperties":false}}},"required":["marketBoards"],"additionalProperties":false}""");
        return new Tool { Name = "market-boards", Title = "Market boards", Description = "Returns market board dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        schema.Ensure(data);
        return new MarketBoardEntity();
    }
}
