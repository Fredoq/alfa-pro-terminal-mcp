using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides account entries retrieval through the router. Usage example: var entries = await new WsAccounts(terminal, logger).Entries(payload).
/// </summary>
public sealed class WsAccounts : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates account entries source. Usage example: var source = new WsAccounts(terminal, logger).
    /// </summary>
    /// <param name="routerSocket">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsAccounts(ITerminal routerSocket, ILogger logger)
    {
        _terminal = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns account entries. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Account entries payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Account entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(payload), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new PayloadArrayEntries(message), new AccountsSchema()), "accounts");
    }
}
