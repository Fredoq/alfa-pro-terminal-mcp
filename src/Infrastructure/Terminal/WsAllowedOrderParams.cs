using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides allowed order parameter entries retrieval through the router. Usage example: var entries = await new WsAllowedOrderParams(terminal, logger).Entries(payload).
/// </summary>
public sealed class WsAllowedOrderParams : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates allowed order parameter entries source. Usage example: var source = new WsAllowedOrderParams(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsAllowedOrderParams(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns allowed order parameter entries. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent();
    /// </summary>
    /// <param name="payload">Allowed order parameter payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Allowed order parameter entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        JsonNode node = JsonNode.Parse(payload.AsString()) ?? throw new InvalidOperationException("Allowed order parameters filter is missing");
        JsonObject root = node.AsObject();
        long group = new JsonInteger(root, "IdObjectGroup").Value();
        long board = new JsonInteger(root, "IdMarketBoard").Value();
        long order = new JsonInteger(root, "IdOrderType").Value();
        long life = new JsonInteger(root, "IdLifeTime").Value();
        long[] types = [1, 2, 7, 8, 9, 10, 11, 12, 13, 28];
        if (Array.IndexOf(types, order) < 0)
        {
            throw new InvalidOperationException("Order type is invalid");
        }
        if (life != 5 && life != 9)
        {
            throw new InvalidOperationException("Order lifetime is invalid");
        }
        string message = await new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new EntityPayload("AllowedOrderParamEntity", true)), _terminal, _logger), _terminal, _logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new FilteredEntries(new PayloadArrayEntries(message), new AllowedOrderParamsScope(group, board, order, life), "Allowed order parameters are missing"), new AllowedOrderParamSchema()), "allowedOrderParams");
    }
}
