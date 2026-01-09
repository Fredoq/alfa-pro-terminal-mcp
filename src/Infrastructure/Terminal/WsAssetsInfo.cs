using System.ComponentModel;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides asset info retrieval through the router. Usage example: var infos = await new WsAssetsInfo(socket, logger).Info(ids, token);.
/// </summary>
public sealed class WsAssetsInfo : IAssetInfos
{
    private readonly IOutboundMessages _outbound;

    public WsAssetsInfo(ITerminal routerSocket, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new AssetInfoEntity()), routerSocket, logger), routerSocket, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    private WsAssetsInfo(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns asset infos for the given identifiers. Usage example: string json = (await info.Info(ids)).Text();.
    /// </summary>
    public async Task<IEntries> Info([Description("Collection of IdObject values to extract")] IEnumerable<long> ids, [Description("Cancellation token controlling the query lifecycle")] CancellationToken cancellationToken = default)
        => new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(await _outbound.NextMessage(cancellationToken)), new AssetIdsScope(ids), "Asset infos are missing"), new AssetInfoSchema());

    /// <summary>
    /// Returns asset infos for the given tickers. Usage example: string json = (await info.InfoByTickers(tickers)).Text();.
    /// </summary>
    public async Task<IEntries> InfoByTickers([Description("Tickers of assets to extract")] IEnumerable<string> tickers, [Description("Cancellation token controlling the query lifecycle")] CancellationToken cancellationToken = default)
        => new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(await _outbound.NextMessage(cancellationToken)), new AssetTickersScope(tickers), "Asset infos are missing"), new AssetInfoSchema());
}
