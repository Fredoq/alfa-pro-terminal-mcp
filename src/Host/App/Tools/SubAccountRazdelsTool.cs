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
/// Provides MCP tool metadata and execution for subaccount portfolios. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class SubAccountRazdelsTool : IMcpTool
{
    private readonly ISubAccountRazdels _items;

    /// <summary>
    /// Creates subaccount portfolios tool with provided entries implementation. Usage example: IMcpTool tool = new SubAccountRazdelsTool(items).
    /// </summary>
    /// <param name="items">Subaccount portfolio entries provider.</param>
    public SubAccountRazdelsTool(ISubAccountRazdels items)
    {
        _items = items;
    }

    /// <summary>
    /// Creates subaccount portfolios tool. Usage example: IMcpTool tool = new SubAccountRazdelsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public SubAccountRazdelsTool(ITerminal terminal, ILogger logger)
        : this(new WsSubAccountRazdels(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "subaccount-razdels";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"subAccountRazdels":{"type":"array","description":"Subaccount portfolio entries","items":{"type":"object","properties":{"IdRazdel":{"type":"integer","description":"Portfolio identifier"},"IdAccount":{"type":"integer","description":"Client account identifier"},"IdSubAccount":{"type":"integer","description":"Client subaccount identifier"},"IdRazdelGroup":{"type":"integer","description":"Portfolio group identifier"},"RCode":{"type":"string","description":"Portfolio code"}},"required":["IdRazdel","IdAccount","IdSubAccount","IdRazdelGroup","RCode"],"additionalProperties":false}}},"required":["subAccountRazdels"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Subaccount portfolios", Description = "Returns subaccount portfolio entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
