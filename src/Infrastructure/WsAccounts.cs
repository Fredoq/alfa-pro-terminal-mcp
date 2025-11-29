using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

public sealed class WsAccounts : IAccounts
{
    private readonly IOutboundMessages _outbound;

    public WsAccounts(IRouterSocket routerSocket, ILogger logger)
        : this(new ClientAccountsEntityResponse(new IncomingMessage(new DataQueryRequest(new ClientAccountsEntity()), routerSocket, logger), routerSocket, logger))
    {
    }

    private WsAccounts(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    public async Task<IAccountsEntries> Entries(CancellationToken cancellationToken = default)
        => new JsonAccountsEntries(await _outbound.NextMessage(cancellationToken));
}
