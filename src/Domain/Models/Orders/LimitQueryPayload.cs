using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Orders;

/// <summary>
/// Represents payload for limit query routing. Usage example: var payload = new LimitQueryPayload(1, 2, 3, 4, 5, 1, 120.5, 2, 3); string json = payload.AsString();.
/// </summary>
public sealed record LimitQueryPayload : IPayload
{
    private readonly (long account, long razdel, long asset, long board, long document, int side, double price, int order, int request) _data;

    /// <summary>
    /// Creates limit query payload. Usage example: var payload = new LimitQueryPayload(1, 2, 3, 4, 5, 1, 120.5, 2, 3).
    /// </summary>
    public LimitQueryPayload(long account, long razdel, long asset, long board, long document, int side, double price, int order, int request)
    {
        _data = (account, razdel, asset, board, document, side, price, order, request);
    }

    /// <summary>
    /// Serializes payload into transport format. Usage example: string json = payload.AsString();.
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { IdAccount = _data.account, IdRazdel = _data.razdel, IdObject = _data.asset, IdMarketBoard = _data.board, IdDocumentType = _data.document, BuySell = _data.side, Price = _data.price, IdOrderType = _data.order, LimitRequestType = _data.request });
}
