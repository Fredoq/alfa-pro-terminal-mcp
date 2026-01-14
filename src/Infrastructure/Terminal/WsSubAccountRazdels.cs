using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides subaccount portfolio entries retrieval through the router. Usage example: var entries = await new WsSubAccountRazdels(terminal, logger).Entries(payload).
/// </summary>
public sealed class WsSubAccountRazdels : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates subaccount portfolio entries source. Usage example: var source = new WsSubAccountRazdels(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsSubAccountRazdels(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns subaccount portfolio entries. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent().
    /// </summary>
    /// <param name="payload">Subaccount portfolio payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Subaccount portfolio entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(payload), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new PayloadArrayEntries(message), new SubAccountRazdelSchema()), "subAccountRazdels");
    }
}
