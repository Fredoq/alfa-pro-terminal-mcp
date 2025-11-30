using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

/// <summary>
/// MCP tool wrapper over terminal behavior. Usage example: var terminal = new McpTerminal(origin); IAccounts accounts = await terminal.Accounts(token);.
/// </summary>
[McpServerToolType]
internal sealed class McpAccounts
{
    private readonly IRouterSocket _routerSocket;
    private readonly ILogger<McpAccounts> _logger;

    /// <summary>
    /// Creates a MCP terminal decorator. Usage example: var terminal = new McpTerminal(origin).
    /// </summary>
    public McpAccounts(IRouterSocket routerSocket, ILogger<McpAccounts> logger)
    {
        ArgumentNullException.ThrowIfNull(routerSocket);
        ArgumentNullException.ThrowIfNull(logger);
        _routerSocket = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns client account entries with identifiers and IIA type. Usage example: string json = await new McpAccounts(socket, logger).Entries().
    /// </summary>
    [McpServerTool, Description("Returns a collection of client accounts. Each account contains an identifier and IIA type.")]
    public async Task<string> Entries() => (await new WsAccounts(_routerSocket, _logger).Entries()).Json();

    /// <summary>
    /// Returns account balance with field descriptions for the specified account id. Usage example: string json = await new McpAccounts(socket, logger).Balance(123).
    /// </summary>
    [McpServerTool, Description("Returns account balance with field descriptions for the given account id.")]
    public async Task<string> Balance(long accountId) => (await new WsBalance(_routerSocket, _logger).Balance(accountId)).Json();
}
