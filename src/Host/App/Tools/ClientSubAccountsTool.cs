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
/// Provides MCP tool metadata and execution for client subaccounts. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class ClientSubAccountsTool : IMcpTool
{
    private readonly IClientSubAccounts _items;

    /// <summary>
    /// Creates client subaccounts tool with provided entries implementation. Usage example: IMcpTool tool = new ClientSubAccountsTool(items).
    /// </summary>
    /// <param name="items">Client subaccounts entries provider.</param>
    public ClientSubAccountsTool(IClientSubAccounts items)
    {
        _items = items;
    }

    /// <summary>
    /// Creates client subaccounts tool. Usage example: IMcpTool tool = new ClientSubAccountsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public ClientSubAccountsTool(ITerminal terminal, ILogger logger)
        : this(new WsClientSubAccounts(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "client-subaccounts";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"clientSubAccounts":{"type":"array","description":"Client subaccount entries","items":{"type":"object","properties":{"IdSubAccount":{"type":"integer","description":"Client subaccount identifier"},"IdAccount":{"type":"integer","description":"Client account identifier"}},"required":["IdSubAccount","IdAccount"],"additionalProperties":false}}},"required":["clientSubAccounts"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Client subaccounts", Description = "Returns client subaccount entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        JsonNode node = (await _items.Entries(token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
