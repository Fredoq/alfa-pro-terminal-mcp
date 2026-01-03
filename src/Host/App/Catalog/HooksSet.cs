using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Catalog;

/// <summary>
/// Provides tool listing, lookup, invocation, and MCP handlers. Usage example: IHooksSet hooks = new Catalog(terminal, factory, content).
/// </summary>
internal sealed class HooksSet : IHooksSet
{
    private readonly IList<IMcpTool> _tools;
    /// <summary>
    /// Creates a tool catalog for MCP operations. Usage example: ICatalog catalog = new Catalog(terminal, factory, content).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="factory">Logger factory.</param>
    /// <param name="content">Response formatter.</param>
    public HooksSet(ITerminal terminal, ILoggerFactory factory, IContent content) : this(
    [
        new AccountsEntriesTool(terminal, factory.CreateLogger<AccountsEntriesTool>(), content),
        new AccountsBalanceTool(terminal, factory.CreateLogger<AccountsBalanceTool>(), content),
        new PositionsTool(terminal, factory.CreateLogger<PositionsTool>(), content),
        new AssetsInfoTool(terminal, factory.CreateLogger<AssetsInfoTool>(), content),
        new AssetsTickersTool(terminal, factory.CreateLogger<AssetsTickersTool>(), content),
        new ArchiveTool(terminal, factory.CreateLogger<ArchiveTool>(), content)
    ])
    {
    }

    public HooksSet(IList<IMcpTool> tools)
    {
        ArgumentNullException.ThrowIfNull(tools);
        _tools = tools;
    }



    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = catalog.Hooks().
    /// </summary>
    public McpServerHandlers Hooks() =>
    new()
    {
        ListToolsHandler = (_, __) => new ValueTask<ListToolsResult>(new ListToolsResult { Tools = [.. _tools.Select(t => t.Tool())] }),
        CallToolHandler = (request, token) =>
        {
            CallToolRequestParams data = request.Params ?? throw new McpProtocolException("Missing call parameters", McpErrorCode.InvalidParams);
            string name = data.Name ?? throw new McpProtocolException("Missing tool name", McpErrorCode.InvalidParams);
            IReadOnlyDictionary<string, JsonElement> items = data.Arguments ?? new Dictionary<string, JsonElement>();
            IMcpTool tool = _tools.FirstOrDefault(t => t.Tool().Name == name) ?? throw new McpProtocolException($"Unknown tool: '{name}'", McpErrorCode.InvalidRequest);
            return tool.Result(items, token);
        }
    };
}
