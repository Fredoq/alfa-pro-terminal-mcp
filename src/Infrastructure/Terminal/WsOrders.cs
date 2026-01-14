using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides orders retrieval through the router. Usage example: var orders = await new WsOrders(socket, logger).Entries(payload, token);.
/// </summary>
public sealed class WsOrders : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates orders retrieval through the router. Usage example: var orders = await new WsOrders(socket, logger).Entries(payload, token);.
    /// </summary>
    /// <param name="routerSocket">Terminal router socket.</param>
    /// <param name="logger">Logger.</param>
    public WsOrders(ITerminal routerSocket, ILogger logger)
    {
        _terminal = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns orders entries for the given payload. Usage example: JsonNode node = (await orders.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Orders query payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Orders entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload.AsString());
        long account = document.RootElement.GetProperty("AccountId").GetInt64();
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new OrderEntity()), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(message), new AccountScope(account), "Account orders are missing"), new OrderSchema()), "orders");
    }
}
