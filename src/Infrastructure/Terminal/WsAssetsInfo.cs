using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides asset info retrieval through the router. Usage example: var infos = await new WsAssetsInfo(socket, logger).Info(ids, token);.
/// </summary>
public sealed class WsAssetsInfo : IAssetInfos
{
    private readonly IOutboundMessages _outbound;

    public WsAssetsInfo(IRouterSocket routerSocket, ILogger logger)
        : this(new Messaging.Responses.AssetInfoEntityResponse(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new AssetInfoEntity()), routerSocket, logger), routerSocket, logger))
    {
    }

    private WsAssetsInfo(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns asset infos for the given identifiers. Usage example: string json = (await info.Info(ids)).Json();.
    /// </summary>
    public async Task<IAssetInfosEntries> Info(IEnumerable<long> ids, CancellationToken cancellationToken = default)
        => new JsonAssetInfosEntries(await _outbound.NextMessage(cancellationToken), ids);
}
