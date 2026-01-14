using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Orders;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides order entry response retrieval through the router. Usage example: JsonNode node = (await new WsOrderEntry(socket, logger).Entry(1, 2, 3, 3, 4, 100.5, 0, 0, 1, 10, "note", 5)).StructuredContent();.
/// </summary>
public sealed class WsOrderEntry : IOrderEntry
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates an order entry source bound to the terminal. Usage example: var source = new WsOrderEntry(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsOrderEntry(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns order entry response for the specified parameters. Usage example: JsonNode node = (await source.Entry(1, 2, 3, 3, 4, 100.5, 0, 0, 1, 10, "note", 5)).StructuredContent();.
    /// </summary>
    /// <param name="account">Client account identifier.</param>
    /// <param name="subaccount">Client subaccount identifier.</param>
    /// <param name="razdel">Portfolio identifier.</param>
    /// <param name="control">Price control type identifier.</param>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="limit">Limit price.</param>
    /// <param name="trigger">Stop price.</param>
    /// <param name="alternative">Alternative limit price.</param>
    /// <param name="side">Trade direction: 1 for buy or -1 for sell.</param>
    /// <param name="quantity">Quantity in units.</param>
    /// <param name="comment">Order comment.</param>
    /// <param name="allowed">Allowed order parameters identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Order entry response entries.</returns>
    public async Task<IEntries> Entry(long account, long subaccount, long razdel, int control, long asset, double limit, double trigger, double alternative, int side, int quantity, string comment, long allowed, CancellationToken token = default)
    {
        string payload = await new TerminalOutboundMessages(new IncomingMessage(new OrderEnterQueryRequest(new OrderEnterQueryPayload(account, subaccount, razdel, control, asset, limit, trigger, alternative, side, quantity, comment, allowed)), _terminal, _logger), _terminal, _logger, new HeartbeatResponse(new QueryResponse("#Order.Enter.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntry(new PayloadObjectEntries(payload), new OrderEntryResponseSchema()), "orderEntry");
    }
}
