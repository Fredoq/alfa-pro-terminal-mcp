using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides orders retrieval through the router. Usage example: var orders = await new WsOrders(socket, logger).Entries(123, token);.
/// </summary>
public sealed class WsOrders : IOrders
{
    private readonly IOutboundMessages _outbound;

    public WsOrders(ITerminal routerSocket, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new OrderEntity()), routerSocket, logger), routerSocket, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    private WsOrders(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns orders entries for the given account. Usage example: JsonNode node = (await orders.Entries(123)).StructuredContent();.
    /// </summary>
    public async Task<IEntries> Entries(long accountId, CancellationToken cancellationToken = default)
        => new RootEntries
            (new SchemaEntries
                (new FilteredEntries
                    (new PayloadArrayEntries
                        (await _outbound.NextMessage(cancellationToken)),
                     new AccountScope(accountId), "Account orders are missing"),
                 new OrderSchema()),
             "orders");
}
