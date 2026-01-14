using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides positions retrieval through the router. Usage example: var positions = await new WsPositions(socket, logger).Entries(payload, token);.
/// </summary>
public sealed class WsPositions : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates positions source. Usage example: var source = new WsPositions(terminal, logger).
    /// </summary>
    /// <param name="routerSocket">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsPositions(ITerminal routerSocket, ILogger logger)
    {
        _terminal = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns positions entries for the given payload. Usage example: JsonNode node = (await positions.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Positions query payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Position entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload.AsString());
        long account = document.RootElement.GetProperty("AccountId").GetInt64();
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new EntityPayload("ClientPositionEntity", true)), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(message), new AccountScope(account), "Account positions are missing"), new PositionSchema()), "positions");
    }
}
