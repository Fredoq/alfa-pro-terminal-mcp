using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812", Justification = "Instantiated by MCP tool discovery")]
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


    [McpServerTool, Description("Returns a collection of client accounts. Each account contains an identifier and IIA type.")]
    public async Task<string> Entries() => (await new WsAccounts(_routerSocket, _logger).Entries()).Json();
}
