using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides MCP tool metadata and execution for limit requests. Usage example: Tool tool = item.Tool().
/// </summary>
internal sealed class LimitRequestTool : IMcpTool
{
    private readonly ILimits _limits;

    /// <summary>
    /// Creates limit request tool with provided limit implementation. Usage example: IMcpTool tool = new LimitRequestTool(limits).
    /// </summary>
    /// <param name="limits">Limit provider.</param>
    public LimitRequestTool(ILimits limits)
    {
        _limits = limits;
    }

    /// <summary>
    /// Creates limit request tool. Usage example: IMcpTool tool = new LimitRequestTool(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public LimitRequestTool(ITerminal terminal, ILogger logger) : this(new WsLimit(terminal, logger))
    {
    }

    /// <summary>
    /// Returns the tool name. Usage example: string name = tool.Name().
    /// </summary>
    public string Name() => "limit-request";

    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = toolItem.Tool().
    /// </summary>
    public Tool Tool()
    {
        JsonElement input = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idAccount":{"type":"integer","description":"Client account identifier"},"idRazdel":{"type":"integer","description":"Portfolio identifier"},"idObject":{"type":"integer","description":"Security identifier"},"idMarketBoard":{"type":"integer","description":"Market identifier"},"idDocumentType":{"type":"integer","description":"Document type identifier"},"buySell":{"type":"integer","description":"Trade direction: 1 for buy or -1 for sell"},"price":{"type":"number","description":"Order price"},"idOrderType":{"type":"integer","description":"Order type identifier: 1 for market or 2 for limit"},"limitRequestType":{"type":"integer","description":"Requested limit type: 3 for free money or 4 for portfolio cost"}},"required":["idAccount","idRazdel","idObject","idMarketBoard","idDocumentType","buySell","price","idOrderType","limitRequestType"]}""");
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"limit":{"type":"object","description":"Limit response for requested order parameters","properties":{"Quantity":{"type":"integer","description":"Available quantity"},"QuantityForOwnAssets":{"type":"integer","description":"Available quantity without leverage"}},"required":["Quantity","QuantityForOwnAssets"],"additionalProperties":false}},"required":["limit"],"additionalProperties":false}""");
        return new Tool { Name = Name(), Title = "Limit request", Description = "Returns available limit for the given order parameters.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Returns the tool execution result for the provided arguments. Usage example: CallToolResult result = await tool.Result(args, token).
    /// </summary>
    public async ValueTask<CallToolResult> Result(IReadOnlyDictionary<string, JsonElement> data, CancellationToken token)
    {
        if (!data.TryGetValue("idAccount", out _))
        {
            throw new McpProtocolException("Missing required argument idAccount", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idRazdel", out _))
        {
            throw new McpProtocolException("Missing required argument idRazdel", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idObject", out _))
        {
            throw new McpProtocolException("Missing required argument idObject", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idMarketBoard", out _))
        {
            throw new McpProtocolException("Missing required argument idMarketBoard", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idDocumentType", out _))
        {
            throw new McpProtocolException("Missing required argument idDocumentType", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("buySell", out _))
        {
            throw new McpProtocolException("Missing required argument buySell", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("price", out _))
        {
            throw new McpProtocolException("Missing required argument price", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("idOrderType", out _))
        {
            throw new McpProtocolException("Missing required argument idOrderType", McpErrorCode.InvalidParams);
        }
        if (!data.TryGetValue("limitRequestType", out _))
        {
            throw new McpProtocolException("Missing required argument limitRequestType", McpErrorCode.InvalidParams);
        }
        JsonNode node = (await _limits.Limit(data["idAccount"].GetInt64(), data["idRazdel"].GetInt64(), data["idObject"].GetInt64(), data["idMarketBoard"].GetInt64(), data["idDocumentType"].GetInt64(), data["buySell"].GetInt32(), data["price"].GetDouble(), data["idOrderType"].GetInt32(), data["limitRequestType"].GetInt32(), token)).StructuredContent();
        return new CallToolResult { StructuredContent = node, Content = [new TextContentBlock { Text = node.ToJsonString() }] };
    }
}
