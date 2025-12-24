using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Routes tool invocations to concrete tools. Usage example: ICalls calls = new Calls(catalog).
/// </summary>
internal sealed class Calls : ICalls
{
    private readonly ICatalog _catalog;

    /// <summary>
    /// Creates a call router backed by a catalog. Usage example: ICalls calls = new Calls(catalog).
    /// </summary>
    /// <param name="catalog">Tool catalog.</param>
    public Calls(ICatalog catalog)
    {
        _catalog = catalog;
    }

    /// <summary>
    /// Returns tool call result. Usage example: CallToolResult result = await calls.Result(request, token).
    /// </summary>
    public ValueTask<CallToolResult> Result(RequestContext<CallToolRequestParams> request, CancellationToken token)
    {
        CallToolRequestParams data = request.Params ?? throw new McpProtocolException("Missing call parameters", McpErrorCode.InvalidParams);
        IReadOnlyDictionary<string, JsonElement> args = data.Arguments ?? new Dictionary<string, JsonElement>();
        IMcpTool tool = _catalog.Tool(data.Name);
        return tool.Result(args, token);
    }
}
