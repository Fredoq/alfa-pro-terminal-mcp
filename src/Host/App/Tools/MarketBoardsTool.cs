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
    /// <summary>
    /// Initializes a new instance of MarketBoardsTool using the given market boards provider.
    /// </summary>
    /// <param name="boards">Provider used to supply market board entries for tool execution.</param>
    public MarketBoardsTool(IMarketBoards boards)
    {
        _boards = boards;
    }

    /// <summary>
    /// Creates market board tool. Usage example: IMcpTool tool = new MarketBoardsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <summary>
    /// Constructs a MarketBoardsTool using the provided terminal and logger.
    /// </summary>
    public MarketBoardsTool(ITerminal terminal, ILogger logger)
        : this(new WsMarketBoards(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// <summary>
/// Gets the name that identifies this MCP tool to the host.
/// </summary>
/// <returns>The tool name "market-boards".</returns>
    public string Name() => "market-boards";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// <summary>
    /// Builds the Tool descriptor for the "market-boards" MCP tool.
    /// </summary>
    /// <returns>A Tool instance containing the tool's metadata: name, title, description, input and output JSON schemas for the marketBoards array, and annotations (ReadOnlyHint=true, IdempotentHint=true, OpenWorldHint=false, DestructiveHint=false).</returns>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"marketBoards":{"type":"array","description":"Market board dictionary entries","items":{"type":"object","properties":{"IdMarketBoard":{"type":"integer","description":"Market board identifier"},"NameMarketBoard":{"type":"string","description":"Market board name"},"DescMarketBoard":{"type":"string","description":"Market board description"},"RCode":{"type":"string","description":"Portfolio code traded on the market board"},"IdObjectCurrency":{"type":"integer","description":"Currency object identifier for the market board"}},"required":["IdMarketBoard","NameMarketBoard","DescMarketBoard","RCode","IdObjectCurrency"],"additionalProperties":false}}},"required":["marketBoards"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Market boards", Description = "Returns market board dictionary entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// <summary>
    /// Fetches market board entries and returns them as structured JSON content.
    /// </summary>
    /// <param name="data">Tool input data (unused by this tool).</param>
    /// <param name="token">Cancellation token to observe while retrieving entries.</param>
    /// <returns>A CallToolResult whose <see cref="CallToolResult.StructuredContent"/> is a <see cref="JsonNode"/> representing the market board entries, and whose <see cref="CallToolResult.Content"/> contains a single <see cref="TextContentBlock"/> with the same content serialized as a JSON string.</returns>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _boards.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}