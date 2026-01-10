using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides fin info params retrieval through the router. Usage example: var entries = await new WsFinInfoParams(terminal, logger).Entries(123, token);.
/// </summary>
public sealed class WsFinInfoParams : IFinInfoParams
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates fin info params source. Usage example: var source = new WsFinInfoParams(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsFinInfoParams(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Returns fin info params entries for a financial instrument. Usage example: JsonNode node = (await source.Entries(123, token)).StructuredContent();.
    /// </summary>
    public async Task<IEntries> Entries(long id, CancellationToken token = default)
    {
        string message = await new TerminalOutboundMessages(new IncomingMessage(new DataQueryRequest(new FinInfoParamsEntity([id])), _terminal, _logger), _terminal, _logger, new HeartbeatResponse(new QueryResponse("#Data.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntries(new PayloadArrayEntries(message), new FinInfoParamsSchema()), "finInfoParams");
    }
}
