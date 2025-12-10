using System.ComponentModel;
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

    public WsAssetsInfo(ITerminal routerSocket, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new AssetInfoEntity()), routerSocket, logger), routerSocket, logger))
    {
    }

    private WsAssetsInfo(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns asset infos for the given identifiers. Usage example: string json = (await info.Info(ids)).Json();.
    /// </summary>
    public async Task<IAssetInfosEntries> Info([Description("Collection of IdObject values to extract")] IEnumerable<long> ids, [Description("Cancellation token controlling the query lifecycle")] CancellationToken cancellationToken = default)
        => new JsonAssetInfosEntries(await _outbound.NextMessage(cancellationToken), new AssetIdsScope(ids));

    /// <summary>
    /// Returns asset infos for the given tickers. Usage example: string json = (await info.InfoByTickers(tickers)).Json();.
    /// </summary>
    public async Task<IAssetInfosEntries> InfoByTickers([Description("Tickers of assets to extract")] IEnumerable<string> tickers, [Description("Cancellation token controlling the query lifecycle")] CancellationToken cancellationToken = default)
        => new JsonAssetInfosEntries(await _outbound.NextMessage(cancellationToken), new AssetTickersScope(tickers));
}
