using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides market board entries retrieval through the router. Usage example: var entries = await new WsMarketBoards(terminal, logger).Entries(token);.
/// </summary>
public sealed class WsMarketBoards : IMarketBoards
{
    private readonly IOutboundMessages _outbound;

    /// <summary>
    /// Creates market board entries source. Usage example: var source = new WsMarketBoards(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <summary>
    /// Initializes a WsMarketBoards configured to retrieve market board data via the terminal's outbound messaging pipeline.
    /// </summary>
    /// <param name="terminal">Terminal used to construct the outbound message pipeline for querying market board data.</param>
    /// <param name="logger">Logger supplied to the outbound messaging components.</param>
    public WsMarketBoards(ITerminal terminal, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new MarketBoardEntity()), terminal, logger), terminal, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    /// <summary>
    /// Creates market board entries source with outbound messages. Usage example: var source = new WsMarketBoards(outbound).
    /// </summary>
    /// <summary>
    /// Initializes a new instance using the provided outbound message source used to retrieve market board payloads.
    /// </summary>
    /// <param name="outbound">Source of outbound messages that Entries will consume to obtain market board data.</param>
    private WsMarketBoards(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns market board entries. Usage example: JsonNode node = (await source.Entries(token)).StructuredContent();.
    /// <summary>
        /// Fetches the next outbound message and returns it as a structured entries tree representing market boards.
        /// </summary>
        /// <param name="token">Token to cancel waiting for the next outbound message.</param>
        /// <returns>An IEntries rooted at "marketBoards" built from the next outbound message payload using the market board schema.</returns>
    public async Task<IEntries> Entries(CancellationToken token = default)
        => new RootEntries(new SchemaEntries(new PayloadArrayEntries(await _outbound.NextMessage(token)), new MarketBoardSchema()), "marketBoards");
}