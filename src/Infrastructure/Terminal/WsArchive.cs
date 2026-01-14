using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides archive candles retrieval through the router. Usage example: JsonNode node = (await new WsArchive(socket, logger).Entries(payload)).StructuredContent();.
/// </summary>
public sealed class WsArchive : IEntriesSource
{
    private readonly ITerminal _socket;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates archive candles source. Usage example: var source = new WsArchive(terminal, logger).
    /// </summary>
    /// <param name="routerSocket">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsArchive(ITerminal routerSocket, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(routerSocket);
        ArgumentNullException.ThrowIfNull(logger);
        _socket = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns archive entries for the specified payload. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Archive query payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Archive entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        string message = await new TerminalOutboundMessages(new IncomingMessage(new ArchiveQueryRequest(payload), _socket, _logger), _socket, _logger, new HeartbeatResponse(new QueryResponse("#Archive.Query"))).NextMessage(token);
        return new RootEntries(new FallbackEntries(new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(message, "OHLCV"), new OhlcvSchema()), "Archive candles are missing"), new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(message, "MPV"), new MpvSchema()), "Archive candles are missing")), "candles");
    }
}
