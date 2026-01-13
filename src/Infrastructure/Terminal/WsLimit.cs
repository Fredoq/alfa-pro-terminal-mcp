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
/// Provides limit retrieval through the router. Usage example: JsonNode node = (await new WsLimit(socket, logger).Limit(1, 2, 3, 4, 5, 1, 120.5, 2, 3)).StructuredContent();.
/// </summary>
public sealed class WsLimit : ILimits
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates a limit source bound to the terminal. Usage example: var source = new WsLimit(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsLimit(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns limit entries for the specified parameters. Usage example: JsonNode node = (await source.Limit(1, 2, 3, 4, 5, 1, 120.5, 2, 3)).StructuredContent();.
    /// </summary>
    /// <param name="account">Client account identifier.</param>
    /// <param name="razdel">Portfolio identifier.</param>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="board">Market board identifier.</param>
    /// <param name="document">Document type identifier.</param>
    /// <param name="side">Trade direction: 1 for buy or -1 for sell.</param>
    /// <param name="price">Order price.</param>
    /// <param name="order">Order type identifier.</param>
    /// <param name="request">Requested limit type identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Limit entries.</returns>
    public async Task<IEntries> Limit(long account, long razdel, long asset, long board, long document, int side, double price, int order, int request, CancellationToken token = default)
    {
        string payload = await new TerminalOutboundMessages(new IncomingMessage(new LimitQueryRequest(new LimitQueryPayload(account, razdel, asset, board, document, side, price, order, request)), _terminal, _logger), _terminal, _logger, new HeartbeatResponse(new QueryResponse("#Order.Limit.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntry(new PayloadObjectEntries(payload, "Value"), new LimitSchema()), "limit");
    }
}
