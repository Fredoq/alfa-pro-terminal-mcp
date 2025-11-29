using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

public sealed class WsAccounts : IAccounts
{
    private readonly IOutboundMessages _outbound;

    public WsAccounts(IRouterSocket routerSocket, ILogger logger)
        : this(new Messaging.Responses.ClientAccountsEntityResponse(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new ClientAccountsEntity()), routerSocket, logger), routerSocket, logger))
    {
    }

    private WsAccounts(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    public async Task<IAccountsEntries> Entries(CancellationToken cancellationToken = default)
        => new JsonAccountsEntries(await _outbound.NextMessage(cancellationToken));
}
