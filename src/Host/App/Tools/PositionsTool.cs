using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides MCP tool metadata and execution for account positions. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class PositionsTool : IMcpTool
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;
    private readonly IContent _content;

    /// <summary>
    /// Creates account positions tool. Usage example: IMcpTool tool = new PositionsTool(terminal, logger, content).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="content">Response formatter.</param>
    public PositionsTool(ITerminal terminal, ILogger logger, IContent content)
    {
        _terminal = terminal;
        _logger = logger;
        _content = content;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "positions";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","description":"Structured tool response","properties":{"data":{"type":"array","description":"Payload entries with field descriptions","items":{"type":"object"}}},"required":["data"]}""");
        return new Tool { Name = Name(), Title = "Account positions", Description = "Returns positions for the given account id with field descriptions.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("accountId", out JsonElement item))
        {
            throw new McpProtocolException("Missing required argument accountId", McpErrorCode.InvalidParams);
        }
        long id = item.GetInt64();
        WsPositions tool = new(_terminal, _logger);
        IEntries entries = await tool.Entries(id, token);
        return _content.Result(entries);
    }
}
