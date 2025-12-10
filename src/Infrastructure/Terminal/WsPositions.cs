using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides positions retrieval through the router. Usage example: var positions = await new WsPositions(socket, logger).Positions(123, token);.
/// </summary>
public sealed class WsPositions : IPositions
{
    private readonly IOutboundMessages _outbound;

    public WsPositions(ITerminal routerSocket, ILogger logger)
        : this(new Messaging.Responses.ClientPositionEntityResponse(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new ClientPositionEntity()), routerSocket, logger), routerSocket, logger))
    {
    }

    private WsPositions(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns positions entries for the given account. Usage example: string json = (await positions.Positions(123)).Json();.
    /// </summary>
    public async Task<IPositionsEntries> Entries(long accountId, CancellationToken cancellationToken = default)
        => new JsonPositionsEntries(await _outbound.NextMessage(cancellationToken), accountId);
}
