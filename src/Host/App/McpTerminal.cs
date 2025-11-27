namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using ModelContextProtocol.Server;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812", Justification = "Instantiated by MCP tool discovery")]
/// <summary>
/// MCP tool wrapper over terminal behavior. Usage example: var terminal = new McpTerminal(origin); IAccounts accounts = await terminal.Accounts(token);.
/// </summary>
[McpServerToolType]
internal sealed class McpTerminal : ITerminal
{
    private readonly ITerminal origin;

    /// <summary>
    /// Creates a MCP terminal decorator. Usage example: var terminal = new McpTerminal(origin).
    /// </summary>
    public McpTerminal(ITerminal terminal)
    {
        ArgumentNullException.ThrowIfNull(terminal);
        origin = terminal;
    }

    /// <summary>
    /// Returns accounts through the decorated terminal. Usage example: IAccounts accounts = await terminal.Accounts(token);.
    /// </summary>
    [McpServerTool]
    public Task<IAccounts> Accounts(CancellationToken cancellationToken) => origin.Accounts(cancellationToken);
}
