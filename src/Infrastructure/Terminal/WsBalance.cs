using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides balance retrieval through the router. Usage example: var entries = await new WsBalance(socket, logger).Entries(payload);.
/// </summary>
public sealed class WsBalance : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates balance source. Usage example: var source = new WsBalance(terminal, logger).
    /// </summary>
    /// <param name="routerSocket">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsBalance(ITerminal routerSocket, ILogger logger)
    {
        _terminal = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns balance entries for the given payload. Usage example: JsonNode node = (await balance.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Balance query payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Balance entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload.AsString());
        long account = document.RootElement.GetProperty("AccountId").GetInt64();
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new EntityPayload("ClientBalanceEntity", true)), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(message), new AccountScope(account), "Account balance is missing"), new AccountBalanceSchema()), "balances");
    }
}
