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
    /// <summary>
    /// Initializes a WsObjectTypes instance using the provided terminal and logger and constructs the default outbound messaging pipeline used to retrieve object type entries.
    /// </summary>
    /// <remarks>
    /// Builds an outbound message source that queries for ObjectTypeEntity data and supplies responses for the Entries method.
    /// </remarks>
    public WsObjectTypes(ITerminal terminal, ILogger logger)
        : this(new Messaging.Responses.TerminalOutboundMessages(new Messaging.Requests.IncomingMessage(new DataQueryRequest(new ObjectTypeEntity()), terminal, logger), terminal, logger, new Messaging.Responses.HeartbeatResponse(new Messaging.Responses.QueryResponse("#Data.Query"))))
    {
    }

    /// <summary>
    /// Creates object type entries source with outbound messages. Usage example: var source = new WsObjectTypes(outbound).
    /// </summary>
    /// <summary>
    /// Initializes a WsObjectTypes instance that uses the provided outbound message stream to obtain object type entries.
    /// </summary>
    /// <param name="outbound">Outbound message stream used to fetch messages supplying object type data.</param>
    private WsObjectTypes(IOutboundMessages outbound)
    {
        _outbound = outbound;
    }

    /// <summary>
    /// Returns object type entries. Usage example: JsonNode node = (await source.Entries(token)).StructuredContent();.
    /// <summary>
        /// Retrieve object type entries from the outbound message pipeline.
        /// </summary>
        /// <param name="token">Cancellation token to cancel waiting for the next outbound message.</param>
        /// <returns>An IEntries tree containing the payload array wrapped with the object type schema and rooted under "objectTypes".</returns>
    public async Task<IEntries> Entries(CancellationToken token = default)
        => new RootEntries(new SchemaEntries(new PayloadArrayEntries(await _outbound.NextMessage(token)), new ObjectTypeSchema()), "objectTypes");
}