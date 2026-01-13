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
/// Provides MCP tool metadata and execution for allowed order parameters. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class AllowedOrderParamsTool : IMcpTool
{
    private readonly IAllowedOrderParams _items;

    /// <summary>
    /// Creates allowed order parameters tool with provided entries implementation. Usage example: IMcpTool tool = new AllowedOrderParamsTool(items).
    /// </summary>
    /// <param name="items">Allowed order parameter entries provider.</param>
    public AllowedOrderParamsTool(IAllowedOrderParams items)
    {
        _items = items;
    }

    /// <summary>
    /// Creates allowed order parameters tool. Usage example: IMcpTool tool = new AllowedOrderParamsTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public AllowedOrderParamsTool(ITerminal terminal, ILogger logger)
        : this(new WsAllowedOrderParams(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "allowed-order-params";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"allowedOrderParams":{"type":"array","description":"Allowed order parameter entries","items":{"type":"object","properties":{"IdAllowedOrderParams":{"type":"integer","description":"Allowed order parameter identifier"},"IdObjectGroup":{"type":"integer","description":"Object group identifier"},"IdMarketBoard":{"type":"integer","description":"Market identifier"},"IdOrderType":{"type":"integer","description":"Order type identifier"},"IdDocumentType":{"type":"integer","description":"Document type identifier"},"IdQuantityType":{"type":"integer","description":"Quantity type identifier"},"IdPriceType":{"type":"integer","description":"Price type identifier"},"IdLifeTime":{"type":"integer","description":"Order lifetime identifier"},"IdExecutionType":{"type":"integer","description":"Execution type identifier"}},"required":["IdAllowedOrderParams","IdObjectGroup","IdMarketBoard","IdOrderType","IdDocumentType","IdQuantityType","IdPriceType","IdLifeTime","IdExecutionType"],"additionalProperties":false}}},"required":["allowedOrderParams"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Allowed order parameters", Description = "Returns allowed order parameter entries.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
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
