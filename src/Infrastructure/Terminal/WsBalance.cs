using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides balance retrieval through the router. Usage example: var balance = await new WsBalance(socket, logger).Balance(123, token);.
/// </summary>
public sealed class WsBalance : IBalances
{
    private readonly IOutboundMessages _outbound;

    public WsBalance(ITerminal routerSocket, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new ClientBalanceEntity()), routerSocket, logger), routerSocket, logger))
    {
    }

    private WsBalance(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns balance entries for the given account. Usage example: string json = (await balance.Balance(123)).Json();.
    /// </summary>
    public async Task<IAccountBalance> Balance(long accountId, CancellationToken cancellationToken = default)
        => new JsonAccountBalance(await _outbound.NextMessage(cancellationToken), accountId);
}
