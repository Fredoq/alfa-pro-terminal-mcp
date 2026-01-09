using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for account entries. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class AccountsEntriesTool : IMcpTool
{
    private readonly IAccounts _accounts;

    /// <summary>
    /// Creates account entries tool with provided accounts implementation. Usage example: IMcpTool tool = new AccountsEntriesTool(accounts).
    /// </summary>
    /// <param name="accounts">Accounts entries provider.</param>
    public AccountsEntriesTool(IAccounts accounts)
    {
        _accounts = accounts;
    }

    /// <summary>
    /// Creates account entries tool. Usage example: IMcpTool tool = new AccountsEntriesTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public AccountsEntriesTool(ITerminal terminal, ILogger logger)
        : this(new WsAccounts(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "entries";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accounts":{"type":"array","description":"List of brokerage accounts available to the user","items":{"type":"object","properties":{"AccountId":{"type":"integer","description":"Unique identifier of the brokerage account used to reference the account in subsequent operations"},"IIAType":{"type":"integer","enum":[0,1,2],"description":"Individual Investment Account type code Values: 0 standard account, 1 IIA Type A, 2 IIA Type B"}},"required":["AccountId","IIAType"],"additionalProperties":false}}},"required":["accounts"],"additionalProperties":false}""");
        return new Tool
        {
            Name = Name(),
            Title = "Accounts entries",
            Description = "Returns a collection of client accounts. Each account contains an identifier and IIA type.",
            InputSchema = input,
            OutputSchema = output,
            Annotations = new ToolAnnotations
            {
                ReadOnlyHint = true,
                IdempotentHint = true,
                OpenWorldHint = false,
                DestructiveHint = false
            }
        };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        IEntries entries = await _accounts.Entries(token);
        JsonNode node = entries.StructuredContent();
        string text = node.ToJsonString();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = text }] };
    }
}
