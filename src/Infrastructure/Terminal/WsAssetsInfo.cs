using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides asset info retrieval through the router. Usage example: var infos = await new WsAssetsInfo(socket, logger).Entries(payload, token);.
/// </summary>
public sealed class WsAssetsInfo : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates asset info source. Usage example: var source = new WsAssetsInfo(terminal, logger).
    /// </summary>
    /// <param name="routerSocket">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsAssetsInfo(ITerminal routerSocket, ILogger logger)
    {
        _terminal = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns asset infos for the given payload. Usage example: JsonNode node = (await info.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Asset info query payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Asset info entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload.AsString());
        JsonElement root = document.RootElement;
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new AssetInfoEntity()), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        if (root.TryGetProperty("IdObjects", out JsonElement ids))
        {
            List<long> items = [];
            foreach (JsonElement value in ids.EnumerateArray())
            {
                items.Add(value.GetInt64());
            }
            return new RootEntries(new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(message), new AssetIdsScope(items), "Asset infos are missing"), new AssetInfoSchema()), "assets");
        }
        if (root.TryGetProperty("Tickers", out JsonElement tickers))
        {
            List<string> items = [];
            foreach (JsonElement value in tickers.EnumerateArray())
            {
                string text = value.GetString() ?? string.Empty;
                if (text.Length == 0)
                {
                    throw new InvalidOperationException("Ticker value is missing");
                }
                items.Add(text);
            }
            return new RootEntries(new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(message), new AssetTickersScope(items), "Asset infos are missing"), new AssetInfoSchema()), "assets");
        }
        throw new InvalidOperationException("Asset filter is missing");
    }
}
