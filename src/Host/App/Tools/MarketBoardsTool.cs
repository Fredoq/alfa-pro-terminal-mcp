using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for market board entries. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class MarketBoardsTool : IMcpTool
{
    private readonly IMarketBoards _boards;

    /// <summary>
    /// Creates market board tool with provided entries implementation. Usage example: IMcpTool tool = new MarketBoardsTool(boards).
    /// </summary>
    /// <param name="boards">Market board entries provider.</param>
    public MarketBoardsTool(IMarketBoards boards)
    {
        _boards = boards;
    }

    /// <summary>
    /// Creates market board tool. Usage example: IMcpTool tool = new MarketBoardsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public MarketBoardsTool(ITerminal terminal, ILogger logger)
        : this(new WsMarketBoards(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "market-boards";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"marketBoards":{"type":"array","description":"Market board dictionary entries","items":{"type":"object","properties":{"IdMarketBoard":{"type":"integer","description":"Market board identifier"},"NameMarketBoard":{"type":"string","description":"Market board name"},"DescMarketBoard":{"type":"string","description":"Market board description"},"RCode":{"type":"string","description":"Portfolio code traded on the market board"},"IdObjectCurrency":{"type":"integer","description":"Currency object identifier for the market board"}},"required":["IdMarketBoard","NameMarketBoard","DescMarketBoard","RCode","IdObjectCurrency"],"additionalProperties":false}}},"required":["marketBoards"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Market boards", Description = "Returns market board dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _boards.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
