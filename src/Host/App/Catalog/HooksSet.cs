using System.Collections.Immutable;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Catalog;

/// <summary>
/// Provides tool listing, lookup, invocation, and MCP handlers. Usage example: IHooksSet hooks = new HooksSet(tools, log).
/// </summary>
internal sealed class HooksSet : IHooksSet, IAsyncDisposable
{
    private readonly IReadOnlyDictionary<string, IMcpTool> _tools;
    private readonly SemaphoreSlim _gate;
    private readonly ILogger<HooksSet> _log;

    /// <summary>
    /// Creates a tool catalog with predefined tools. Usage example: IHooksSet hooks = new HooksSet(tools, log).
    /// </summary>
    /// <param name="tools">Tool list.</param>
    /// <param name="log">Logger.</param>
    public HooksSet(IList<IMcpTool> tools, ILogger<HooksSet> log)
    {
        ArgumentNullException.ThrowIfNull(tools);
        ArgumentNullException.ThrowIfNull(log);
        _tools = tools.ToDictionary(t => t.Name(), StringComparer.Ordinal);
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
                IDictionary<string, JsonElement> items = data.Arguments ?? ImmutableDictionary<string, JsonElement>.Empty;
                if (name == "order-enter")
                {
                    string text = JsonSerializer.Serialize(items);
                    ElicitRequestParams prompt = new()
                    {
                        Message = $"Confirm order entry with parameters: {text}"
                    };
                    ElicitResult answer = await request.Server.ElicitAsync(prompt, token);
                    if (!answer.IsAccepted)
                    {
                        throw new McpProtocolException("Order entry confirmation was rejected", McpErrorCode.InvalidRequest);
                    }
                }
                if (name == "order-cancel")
                {
                    string text = JsonSerializer.Serialize(items);
                    ElicitRequestParams prompt = new()
                    {
                        Message = $"Confirm order cancel with parameters: {text}"
                    };
                    ElicitResult answer = await request.Server.ElicitAsync(prompt, token);
                    if (!answer.IsAccepted)
                    {
                        throw new McpProtocolException("Order cancel confirmation was rejected", McpErrorCode.InvalidRequest);
                    }
                }
                IMcpTool tool = _tools.TryGetValue(name, out IMcpTool? item) ? item : throw new McpProtocolException($"Unknown tool: '{name}'", McpErrorCode.InvalidRequest);
                return await tool.Result(items.AsReadOnly(), token);
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
