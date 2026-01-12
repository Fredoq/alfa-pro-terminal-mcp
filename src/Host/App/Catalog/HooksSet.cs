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
/// Provides tool listing, lookup, invocation, and MCP handlers. Usage example: IHooksSet hooks = new Catalog(terminal, factory).
/// </summary>
internal sealed class HooksSet : IHooksSet, IAsyncDisposable
{
    private readonly IList<IMcpTool> _tools;
    private readonly SemaphoreSlim _gate;
    /// <summary>
    /// Creates a tool catalog for MCP operations. Usage example: ICatalog catalog = new Catalog(terminal, factory).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="factory">Logger factory.</param>
    public HooksSet(ITerminal terminal, ILoggerFactory factory) : this(
    [
        new AccountsEntriesTool(terminal, factory.CreateLogger<AccountsEntriesTool>()),
        new AccountsBalanceTool(terminal, factory.CreateLogger<AccountsBalanceTool>()),
        new PositionsTool(terminal, factory.CreateLogger<PositionsTool>()),
        new AssetsInfoTool(terminal, factory.CreateLogger<AssetsInfoTool>()),
        new AssetsTickersTool(terminal, factory.CreateLogger<AssetsTickersTool>()),
        new ObjectTypesTool(terminal, factory.CreateLogger<ObjectTypesTool>()),
        new ObjectGroupsTool(terminal, factory.CreateLogger<ObjectGroupsTool>()),
        new MarketBoardsTool(terminal, factory.CreateLogger<MarketBoardsTool>()),
        new ArchiveTool(terminal, factory.CreateLogger<ArchiveTool>())
    ])
    {
    }

    /// <summary>
    /// Creates a tool catalog with predefined tools. Usage example: IHooksSet hooks = new HooksSet(tools).
    /// </summary>
    /// <param name="tools">Tool list.</param>
    public HooksSet(IList<IMcpTool> tools)
    {
        ArgumentNullException.ThrowIfNull(tools);
        _tools = tools;
        _gate = new SemaphoreSlim(1, 1);
    }



    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = catalog.Hooks().
    /// </summary>
    public McpServerHandlers Hooks() =>
    new()
    {
        ListToolsHandler = (_, __) => new ValueTask<ListToolsResult>(new ListToolsResult { Tools = [.. _tools.Select(t => t.Tool())] }),
        CallToolHandler = async (request, token) =>
        {
            await _gate.WaitAsync(token);
            try
            {
                CallToolRequestParams data = request.Params ?? throw new McpProtocolException("Missing call parameters", McpErrorCode.InvalidParams);
                string name = data.Name ?? throw new McpProtocolException("Missing tool name", McpErrorCode.InvalidParams);
                IReadOnlyDictionary<string, JsonElement> items = data.Arguments ?? new Dictionary<string, JsonElement>();
                IMcpTool tool = _tools.FirstOrDefault(t => t.Tool().Name == name) ?? throw new McpProtocolException($"Unknown tool: '{name}'", McpErrorCode.InvalidRequest);
                return await tool.Result(items, token);
            }
            finally
            {
                _gate.Release();
            }
        }
    };
    /// <summary>
    /// Disposes the gate after acquiring it to prevent further tool calls during shutdown.
    /// </summary>
    /// <remarks>
    /// This method keeps the semaphore held to block further acquisitions while disposing.
    /// </remarks>
    /// <exception cref="TimeoutException">Thrown when the gate cannot be acquired within the shutdown window.</exception>
    public async ValueTask DisposeAsync()
    {
        bool result = await _gate.WaitAsync(TimeSpan.FromSeconds(30));
        if (!result)
        {
            throw new TimeoutException("Semaphore wait timed out during shutdown");
        }
        _gate.Dispose();
    }
}
