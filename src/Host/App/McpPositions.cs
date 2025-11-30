using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

/// <summary>
/// MCP tool wrapper for account positions. Usage example: string json = await new McpPositions(socket, logger).Positions(123).
/// </summary>
[McpServerToolType]
internal sealed class McpPositions
{
    private readonly IRouterSocket _routerSocket;
    private readonly ILogger<McpPositions> _logger;

    /// <summary>
    /// Creates a MCP positions tool. Usage example: var tool = new McpPositions(socket, logger).
    /// </summary>
    public McpPositions(IRouterSocket routerSocket, ILogger<McpPositions> logger)
    {
        ArgumentNullException.ThrowIfNull(routerSocket);
        ArgumentNullException.ThrowIfNull(logger);
        _routerSocket = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns positions for the specified account with field descriptions. Usage example: string json = await tool.Positions(123).
    /// </summary>
    [McpServerTool, Description("Returns positions for the given account id with field descriptions.")]
    public async Task<string> Positions(long accountId) => (await new WsPositions(_routerSocket, _logger).Entries(accountId)).Json();

    /// <summary>
    /// Returns asset infos for the provided identifiers with field descriptions. Usage example: string json = await tool.Info(new[] { 1L, 2L }).
    /// </summary>
    [McpServerTool, Description("Returns asset info list for the given object identifiers with field descriptions.")]
    public async Task<string> Info(long[] idObjects) => (await new WsAssetsInfo(_routerSocket, _logger).Info(idObjects)).Json();
}
