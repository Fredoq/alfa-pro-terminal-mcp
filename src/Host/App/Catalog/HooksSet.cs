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
    /// <summary>
    /// Initializes a HooksSet with the standard catalog of MCP tools created using the provided terminal and per-tool loggers.
    /// </summary>
    /// <param name="terminal">Terminal instance supplied to each tool.</param>
    /// <param name="factory">Logger factory used to create a per-tool logger for each tool.</param>
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
    /// <summary>
    /// Creates a HooksSet that exposes the provided MCP tools and initializes internal synchronization.
    /// </summary>
    /// <param name="tools">List of tool implementations to expose; must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tools"/> is null.</exception>
    public HooksSet(IList<IMcpTool> tools)
    {
        ArgumentNullException.ThrowIfNull(tools);
        _tools = tools;
        _gate = new SemaphoreSlim(1, 1);
    }



    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = catalog.Hooks().
    /// <summary>
    /// Provide MCP server handlers for listing available tools and invoking a tool by name.
    /// </summary>
    /// <returns>
    /// A <see cref="McpServerHandlers"/> instance whose <c>ListToolsHandler</c> returns the catalog derived from the internal tools list,
    /// and whose <c>CallToolHandler</c> invokes the requested tool by name and returns that tool's result.
    /// </returns>
    /// <exception cref="McpProtocolException">
    /// Thrown by the <c>CallToolHandler</c> when required call parameters are missing, when the tool name is missing, or when no tool with the specified name exists.
    /// </exception>
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
    /// Asynchronously waits for any in-progress tool invocation to finish and then frees the internal synchronization resource.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that completes after the internal semaphore has been acquired and disposed.</returns>
    public async ValueTask DisposeAsync()
    {
        await _gate.WaitAsync();
        _gate.Dispose();
    }
}