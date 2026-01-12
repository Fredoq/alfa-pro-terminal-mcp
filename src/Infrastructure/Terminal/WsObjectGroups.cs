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
/// Provides object group entries retrieval through the router. Usage example: var entries = await new WsObjectGroups(terminal, logger).Entries(token);.
/// </summary>
public sealed class WsObjectGroups : IObjectGroups
{
    private readonly IOutboundMessages _outbound;

    /// <summary>
    /// Creates object group entries source. Usage example: var source = new WsObjectGroups(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <summary>
    /// Initializes a WsObjectGroups instance wired to the provided terminal and logger using the default outbound message pipeline.
    /// </summary>
    /// <param name="terminal">Terminal used to route outbound and inbound messages for querying object group entries.</param>
    /// <param name="logger">Logger used by the outbound messaging pipeline.</param>
    public WsObjectGroups(ITerminal terminal, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new ObjectGroupEntity()), terminal, logger), terminal, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    /// <summary>
    /// Creates object group entries source with outbound messages. Usage example: var source = new WsObjectGroups(outbound).
    /// </summary>
    /// <summary>
    /// Initializes a new instance that uses the provided outbound message stream.
    /// </summary>
    /// <param name="outbound">The outbound message stream used to request and receive object group entries.</param>
    private WsObjectGroups(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns object group entries. Usage example: JsonNode node = (await source.Entries(token)).StructuredContent();.
    /// <summary>
        /// Retrieves the object group entries from the outbound message stream.
        /// </summary>
        /// <param name="token">A cancellation token to cancel the retrieval operation.</param>
        /// <returns>An IEntries representing the "objectGroups" root containing entries that conform to the ObjectGroupSchema.</returns>
    public async Task<IEntries> Entries(CancellationToken token = default)
        => new RootEntries(new SchemaEntries(new PayloadArrayEntries(await _outbound.NextMessage(token)), new ObjectGroupSchema()), "objectGroups");
}