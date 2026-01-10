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
/// Provides object type entries retrieval through the router. Usage example: var entries = await new WsObjectTypes(terminal, logger).Entries(token);.
/// </summary>
public sealed class WsObjectTypes : IObjectTypes
{
    private readonly IOutboundMessages _outbound;

    /// <summary>
    /// Creates object type entries source. Usage example: var source = new WsObjectTypes(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsObjectTypes(ITerminal terminal, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new ObjectTypeEntity()), terminal, logger), terminal, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    /// <summary>
    /// Creates object type entries source with outbound messages. Usage example: var source = new WsObjectTypes(outbound).
    /// </summary>
    /// <param name="outbound">Outbound message stream.</param>
    private WsObjectTypes(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns object type entries. Usage example: JsonNode node = (await source.Entries(token)).StructuredContent();.
    /// </summary>
    public async Task<IEntries> Entries(CancellationToken token = default)
        => new RootEntries(new SchemaEntries(new PayloadArrayEntries(await _outbound.NextMessage(token)), new ObjectTypeSchema()), "objectTypes");
}
