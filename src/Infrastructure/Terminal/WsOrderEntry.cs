using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Trading;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Provides order entry response retrieval through the router. Usage example: JsonNode node = (await new WsOrderEntry(socket, logger).Entries(payload)).StructuredContent();.
/// </summary>
public sealed class WsOrderEntry : IEntriesSource
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;
    private readonly WsSubaccount _subaccount;
    private readonly WsInstrument _instrument;
    private readonly WsRazdel _razdel;
    private readonly WsAllowedOrderParam _param;

    /// <summary>
    /// Creates an order entry source bound to the terminal. Usage example: var source = new WsOrderEntry(terminal, logger).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="logger">Logger instance.</param>
    public WsOrderEntry(ITerminal terminal, ILogger logger)
    {
        _terminal = terminal;
        _logger = logger;
        _subaccount = new WsSubaccount(terminal, logger);
        _instrument = new WsInstrument(terminal, logger);
        _razdel = new WsRazdel(terminal, logger);
        _param = new WsAllowedOrderParam(terminal, logger);
    }

    /// <summary>
    /// Returns order entry response for the specified payload. Usage example: JsonNode node = (await source.Entries(payload)).StructuredContent();.
    /// </summary>
    /// <param name="payload">Order entry payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Order entry response entries.</returns>
    public async Task<IEntries> Entries(IPayload payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        using JsonDocument document = JsonDocument.Parse(payload.AsString());
        JsonElement root = document.RootElement;
        long account = root.GetProperty("IdAccount").GetInt64();
        int control = root.GetProperty("IdPriceControlType").GetInt32();
        long asset = root.GetProperty("IdObject").GetInt64();
        double limit = root.GetProperty("LimitPrice").GetDouble();
        double stop = root.GetProperty("StopPrice").GetDouble();
        double alternative = root.GetProperty("LimitLevelAlternative").GetDouble();
        int side = root.GetProperty("BuySell").GetInt32();
        int quantity = root.GetProperty("Quantity").GetInt32();
        string comment = root.GetProperty("Comment").GetString() ?? throw new InvalidOperationException("Comment is missing");
        long subaccount = root.TryGetProperty("IdSubAccount", out JsonElement data) ? data.GetInt64() : await _subaccount.Identifier(account, token);
        bool razdel = root.TryGetProperty("IdRazdel", out JsonElement node);
        bool param = root.TryGetProperty("IdAllowedOrderParams", out JsonElement item);
        bool flag = !razdel || !param;
        InstrumentValue instrument = flag ? await _instrument.Value(asset, token) : new InstrumentValue(0, 0, string.Empty);
        long portfolio = razdel ? node.GetInt64() : await _razdel.Identifier(account, subaccount, instrument.Code, token);
        long combo = param ? item.GetInt64() : await _param.Identifier(instrument.Group, instrument.Market, limit, token);
        string body = JsonSerializer.Serialize(new { IdAccount = account, IdSubAccount = subaccount, IdRazdel = portfolio, IdPriceControlType = control, IdObject = asset, LimitPrice = limit, StopPrice = stop, LimitLevelAlternative = alternative, BuySell = side, Quantity = quantity, Comment = comment, IdAllowedOrderParams = combo });
        string text = await new TerminalOutboundMessages(new IncomingMessage(new OrderEnterQueryRequest(new TextPayload(body)), _terminal, _logger), _terminal, _logger, new HeartbeatResponse(new QueryResponse("#Order.Enter.Query"))).NextMessage(token);
        return new RootEntries(new SchemaEntry(new PayloadObjectEntries(text), new OrderEntryResponseSchema()), "orderEntry");
    }
}
