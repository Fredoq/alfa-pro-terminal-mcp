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
/// Provides subaccount portfolio entries retrieval through the router. Usage example: var entries = await new WsSubAccountRazdels(terminal, logger).Entries(token).
/// </summary>
public sealed class WsSubAccountRazdels : ISubAccountRazdels
{
    private readonly IOutboundMessages _outbound;

    /// <summary>
    /// Creates subaccount portfolio entries source. Usage example: var source = new WsSubAccountRazdels(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsSubAccountRazdels(ITerminal terminal, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new SubAccountRazdelEntity()), terminal, logger), terminal, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    /// <summary>
    /// Creates subaccount portfolio entries source with outbound messages. Usage example: var source = new WsSubAccountRazdels(outbound).
    /// </summary>
    /// <param name="outbound">Outbound message stream.</param>
    private WsSubAccountRazdels(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns subaccount portfolio entries. Usage example: JsonNode node = (await source.Entries(token)).StructuredContent().
    /// </summary>
    public async Task<IEntries> Entries(CancellationToken token = default)
        => new RootEntries(new SchemaEntries(new PayloadArrayEntries(await _outbound.NextMessage(token)), new SubAccountRazdelSchema()), "subAccountRazdels");
}
