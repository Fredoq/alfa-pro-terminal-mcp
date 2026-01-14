using System.Collections.Immutable;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Catalog;

/// <summary>
/// Provides tool listing, lookup, invocation, and MCP handlers. Usage example: IHooksSet hooks = new Catalog(terminal, factory).
/// </summary>
internal sealed class HooksSet : IHooksSet, IAsyncDisposable
{
    private readonly IReadOnlyDictionary<string, IMcpTool> _tools;
    private readonly SemaphoreSlim _gate;
    private readonly ILogger<HooksSet> _log;

    /// <summary>
    /// Creates a tool catalog for MCP operations. Usage example: ICatalog catalog = new Catalog(terminal, factory).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="factory">Logger factory.</param>
    public HooksSet(ITerminal terminal, ILoggerFactory factory) : this(
    [
        new McpTool(new WsAccounts(terminal, factory.CreateLogger<WsAccounts>()), new AccountsEntriesPlan()),
        new McpTool(new WsClientSubAccounts(terminal, factory.CreateLogger<WsClientSubAccounts>()), new ClientSubAccountsPlan()),
        new McpTool(new WsSubAccountRazdels(terminal, factory.CreateLogger<WsSubAccountRazdels>()), new SubAccountRazdelsPlan()),
        new McpTool(new WsAllowedOrderParams(terminal, factory.CreateLogger<WsAllowedOrderParams>()), new AllowedOrderParamsPlan()),
        new McpTool(new WsBalance(terminal, factory.CreateLogger<WsBalance>()), new AccountsBalancePlan()),
        new McpTool(new WsPositions(terminal, factory.CreateLogger<WsPositions>()), new PositionsPlan()),
        new McpTool(new WsOrders(terminal, factory.CreateLogger<WsOrders>()), new OrdersPlan()),
        new McpTool(new WsOrderEntry(terminal, factory.CreateLogger<WsOrderEntry>()), new OrderEntryPlan()),
        new McpTool(new WsLimit(terminal, factory.CreateLogger<WsLimit>()), new LimitRequestPlan()),
        new McpTool(new WsAssetsInfo(terminal, factory.CreateLogger<WsAssetsInfo>()), new AssetsInfoPlan()),
        new McpTool(new WsAssetsInfo(terminal, factory.CreateLogger<WsAssetsInfo>()), new AssetsTickersPlan()),
        new McpTool(new WsObjectTypes(terminal, factory.CreateLogger<WsObjectTypes>()), new ObjectTypesPlan()),
        new McpTool(new WsObjectGroups(terminal, factory.CreateLogger<WsObjectGroups>()), new ObjectGroupsPlan()),
        new McpTool(new WsMarketBoards(terminal, factory.CreateLogger<WsMarketBoards>()), new MarketBoardsPlan()),
        new McpTool(new WsArchive(terminal, factory.CreateLogger<WsArchive>()), new ArchivePlan())
    ], factory.CreateLogger<HooksSet>())
    {
    }

    /// <summary>
    /// Creates a tool catalog with predefined tools. Usage example: IHooksSet hooks = new HooksSet(tools).
    /// </summary>
    /// <param name="tools">Tool list.</param>
    public HooksSet(IList<IMcpTool> tools) : this(tools, NullLogger<HooksSet>.Instance)
    {
    }

    /// <summary>
    /// Creates a tool catalog with predefined tools and logging. Usage example: IHooksSet hooks = new HooksSet(tools, log).
    /// </summary>
    /// <param name="tools">Tool list.</param>
    /// <param name="log">Logger.</param>
    public HooksSet(IList<IMcpTool> tools, ILogger<HooksSet> log) : this(
        tools.ToDictionary(t => t.Name(), StringComparer.Ordinal),
        log)
    {
    }

    /// <summary>
    /// Creates a tool catalog with predefined tools and logging. Usage example: IHooksSet hooks = new HooksSet(tools, log).
    /// </summary>
    /// <param name="tools">Tool map.</param>
    /// <param name="log">Logger.</param>
    public HooksSet(IReadOnlyDictionary<string, IMcpTool> tools, ILogger<HooksSet> log)
    {
        ArgumentNullException.ThrowIfNull(tools);
        ArgumentNullException.ThrowIfNull(log);
        _tools = tools;
        _gate = new SemaphoreSlim(1, 1);
        _log = log;
    }



    /// <summary>
    /// Returns hooks. Usage example: McpServerHandlers data = catalog.Hooks().
    /// </summary>
    public McpServerHandlers Hooks() =>
    new()
    {
        ListToolsHandler = (_, _) => new ValueTask<ListToolsResult>(new ListToolsResult { Tools = [.. _tools.Values.Select(t => t.Tool())] }),
        CallToolHandler = async (request, token) =>
        {
            await _gate.WaitAsync(token);
            try
            {
                CallToolRequestParams data = request.Params ?? throw new McpProtocolException("Missing call parameters", McpErrorCode.InvalidParams);
                string name = data.Name ?? throw new McpProtocolException("Missing tool name", McpErrorCode.InvalidParams);
                IReadOnlyDictionary<string, JsonElement> items = data.Arguments ?? ImmutableDictionary<string, JsonElement>.Empty;
                if (name == "order-enter")
                {
                    string text = JsonSerializer.Serialize(items);
                    ElicitRequestParams prompt = new()
                    {
                        Message = $"Confirm order entry with parameters: {text}",
                        RequestedSchema = new ElicitRequestParams.RequestSchema { Properties = new Dictionary<string, ElicitRequestParams.PrimitiveSchemaDefinition> { ["confirm"] = new ElicitRequestParams.BooleanSchema { Type = "boolean", Title = "Confirm order entry", Description = "Confirm order entry with provided parameters", Default = false } }, Required = ["confirm"] }
                    };
                    ElicitResult answer = await request.Server.ElicitAsync(prompt, token);
                    if (!answer.IsAccepted || answer.Content is null || !answer.Content.TryGetValue("confirm", out JsonElement value) || !value.GetBoolean())
                    {
                        throw new McpProtocolException("Order entry confirmation was rejected", McpErrorCode.InvalidRequest);
                    }
                }
                IMcpTool tool = _tools.TryGetValue(name, out IMcpTool? item) ? item : throw new McpProtocolException($"Unknown tool: '{name}'", McpErrorCode.InvalidRequest);
                return await tool.Result(items, token);
            }
            finally
            {
                try
                {
                    _gate.Release();
                }
                catch (ObjectDisposedException)
                {
                    // Semaphore disposed during shutdown - safe to ignore
                }
            }
        }
    };
    /// <summary>
    /// Disposes the gate after a bounded wait during shutdown. Usage example: await hooks.DisposeAsync().
    /// </summary>
    /// <remarks>
    /// This method always disposes the gate after the wait, then logs when the wait times out.
    /// </remarks>
    public async ValueTask DisposeAsync()
    {
        bool result = false;
        try
        {
            result = await _gate.WaitAsync(TimeSpan.FromSeconds(30));
        }
        finally
        {
            _gate.Dispose();
        }
        if (!result)
        {
            _log.LogWarning("Semaphore wait timed out during shutdown");
        }
    }
}
