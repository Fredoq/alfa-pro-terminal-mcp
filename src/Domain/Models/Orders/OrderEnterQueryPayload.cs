using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Orders;

/// <summary>
/// Represents payload for order entry query routing. Usage example: var payload = new OrderEnterQueryPayload(1, 2, 3, 3, 4, 100.5, 0, 0, 1, 10, "note", 5); string json = payload.AsString();.
/// </summary>
public sealed record OrderEnterQueryPayload : IPayload
{
    private readonly (long account, long subaccount, long razdel, int control, long asset, double limit, double trigger, double alternative, int side, int quantity, string comment, long allowed) _data;

    /// <summary>
    /// Creates order entry query payload. Usage example: var payload = new OrderEnterQueryPayload(1, 2, 3, 3, 4, 100.5, 0, 0, 1, 10, "note", 5).
    /// </summary>
    public OrderEnterQueryPayload(long account, long subaccount, long razdel, int control, long asset, double limit, double trigger, double alternative, int side, int quantity, string comment, long allowed)
    {
        _data = (account, subaccount, razdel, control, asset, limit, trigger, alternative, side, quantity, comment, allowed);
    }

    /// <summary>
    /// Serializes payload into transport format. Usage example: string json = payload.AsString();.
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { IdAccount = _data.account, IdSubAccount = _data.subaccount, IdRazdel = _data.razdel, IdPriceControlType = _data.control, IdObject = _data.asset, LimitPrice = _data.limit, StopPrice = _data.trigger, LimitLevelAlternative = _data.alternative, BuySell = _data.side, Quantity = _data.quantity, Comment = _data.comment, IdAllowedOrderParams = _data.allowed });
}
