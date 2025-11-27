using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

internal sealed class WsAccounts : IAccounts
{
    private readonly IOutboundMessages _outbound;

    public WsAccounts(IRouterSocket routerSocket)
        : this(new ClientAccountsEntityResponse(new IncomingMessage(new QueryRequest(new QuotedPayload(new ClientAccountsEntity())), routerSocket), routerSocket))
    {
    }

    private WsAccounts(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    public async Task<IAccountsEntries> Entries(CancellationToken cancellationToken)
        => new JsonAccountsEntries(await _outbound.NextMessage(cancellationToken));
}
