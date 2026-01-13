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
/// Provides allowed order parameter entries retrieval through the router. Usage example: var entries = await new WsAllowedOrderParams(terminal, logger).Entries(token).
/// </summary>
public sealed class WsAllowedOrderParams : IAllowedOrderParams
{
    private readonly IOutboundMessages _outbound;

    /// <summary>
    /// Creates allowed order parameter entries source. Usage example: var source = new WsAllowedOrderParams(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsAllowedOrderParams(ITerminal terminal, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new AllowedOrderParamEntity()), terminal, logger), terminal, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    /// <summary>
    /// Creates allowed order parameter entries source with outbound messages. Usage example: var source = new WsAllowedOrderParams(outbound).
    /// </summary>
    /// <param name="outbound">Outbound message stream.</param>
    private WsAllowedOrderParams(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns allowed order parameter entries. Usage example: JsonNode node = (await source.Entries(token)).StructuredContent().
    /// </summary>
    public async Task<IEntries> Entries(CancellationToken token = default)
        => new RootEntries(new SchemaEntries(new PayloadArrayEntries(await _outbound.NextMessage(token)), new AllowedOrderParamSchema()), "allowedOrderParams");
}
