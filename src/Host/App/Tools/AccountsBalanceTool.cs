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
/// Provides MCP tool metadata and execution for account balance. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class AccountsBalanceTool : IMcpTool
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;
    private readonly IContent _content;

    /// <summary>
    /// Creates account balance tool. Usage example: IMcpTool tool = new AccountsBalanceTool(terminal, logger, content).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="content">Response formatter.</param>
    public AccountsBalanceTool(ITerminal terminal, ILogger logger, IContent content)
    {
        _terminal = terminal;
        _logger = logger;
        _content = content;
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "balance";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accountId":{"type":"integer","description":"Account identifier"}},"required":["accountId"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","description":"Structured tool response","properties":{"data":{"type":"array","description":"Payload entries with field descriptions","items":{"type":"object"}}},"required":["data"]}""");
        return new Tool { Name = Name(), Title = "Account balance", Description = "Returns account balance with field descriptions for the given account id.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
        WsBalance tool = new(_terminal, _logger);
        IEntries entries = await tool.Balance(id, token);
        return _content.Result(entries);
    }
}
