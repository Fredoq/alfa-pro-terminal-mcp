using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines MCP tool invocation behavior. Usage example: CallToolResult result = await calls.Result(request, token).
/// </summary>
public interface ICalls
{
    /// <summary>
    /// Returns tool call result. Usage example: CallToolResult result = await calls.Result(request, token).
    /// </summary>
    ValueTask<CallToolResult> Result(RequestContext<CallToolRequestParams> request, CancellationToken token);
}
