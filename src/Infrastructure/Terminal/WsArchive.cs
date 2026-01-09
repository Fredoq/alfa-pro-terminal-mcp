using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Archive;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Archive;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides archive candles retrieval through the router. Usage example: string json = (await new WsArchive(socket, logger).History(1, 0, "hour", 1, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow)).Text();.
/// </summary>
public sealed class WsArchive : IArchive
{
    private readonly ITerminal _socket;
    private readonly ILogger _logger;

    public WsArchive(ITerminal routerSocket, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(routerSocket);
        ArgumentNullException.ThrowIfNull(logger);
        _socket = routerSocket;
        _logger = logger;
    }

    public async Task<IEntries> History(long idFi, int candleType, string interval, int period, DateTime firstDay, DateTime lastDay, CancellationToken cancellationToken = default)
    {
        ArchiveQueryPayload payload = new(idFi, candleType, interval, period, firstDay, lastDay);
        ArchiveQueryRequest routing = new(payload);
        TerminalOutboundMessages outbound = new(new IncomingMessage(routing, _socket, _logger), _socket, _logger, new HeartbeatResponse(new QueryResponse("#Archive.Query")));
        string message = await outbound.NextMessage(cancellationToken);
        return new FallbackEntries(new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(message, "OHLCV"), new OhlcvSchema()), "Archive candles are missing"), new RequiredEntries(new SchemaEntries(new PayloadArrayEntries(message, "MPV"), new MpvSchema()), "Archive candles are missing"));
    }
}
