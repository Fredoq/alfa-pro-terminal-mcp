using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines tool catalog behavior for MCP handlers. Usage example: ListToolsResult list = await catalog.Tools(request, token).
/// </summary>
internal interface ICatalog
{
    /// <summary>
    /// Returns list tools response. Usage example: ListToolsResult list = await catalog.Tools(request, token).
    /// </summary>
    ValueTask<ListToolsResult> Tools(RequestContext<ListToolsRequestParams> request, CancellationToken token);

    /// <summary>
    /// Returns a tool by name. Usage example: IMcpTool tool = catalog.Tool(name).
    /// </summary>
    IMcpTool Tool(string name);
}
