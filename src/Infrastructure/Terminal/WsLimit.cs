using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides limit retrieval through the router. Usage example: JsonNode node = (await new WsLimit(socket, logger).Entries(payload)).StructuredContent();.
/// </summary>
public sealed class WsLimit : IEntriesSource
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
    /// Returns limit entries for the specified payload. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Limit query payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Limit entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        string text = await new TerminalOutboundMessages(new IncomingMessage(new LimitQueryRequest(payload), _terminal, _logger), _terminal, _logger, new HeartbeatResponse(new QueryResponse("#Order.Limit.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntry(new PayloadObjectEntries(text, "Value"), new LimitSchema()), "limit");
    }
}
