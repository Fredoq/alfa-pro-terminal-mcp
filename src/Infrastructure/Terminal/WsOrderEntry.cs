using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides order entry response retrieval through the router. Usage example: JsonNode node = (await new WsOrderEntry(socket, logger).Entries(payload)).StructuredContent();.
/// </summary>
public sealed class WsOrderEntry : IEntriesSource
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
    /// Returns order entry response for the specified payload. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Order entry payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Order entry response entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        string text = await new TerminalOutboundMessages(new IncomingMessage(new OrderEnterQueryRequest(payload), _terminal, _logger), _terminal, _logger, new HeartbeatResponse(new QueryResponse("#Order.Enter.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntry(new PayloadObjectEntries(text), new OrderEntryResponseSchema()), "orderEntry");
    }
}
