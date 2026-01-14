using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Order entry response interface. Usage example: var entries = await order.Entry(1, 2, 3, 3, 4, 100.5, 0, 0, 1, 10, "note", 5, token).
/// </summary>
public interface IOrderEntry
{
    /// <summary>
    /// Returns order entry response for the specified parameters. Usage example: var entries = await order.Entry(1, 2, 3, 3, 4, 100.5, 0, 0, 1, 10, "note", 5, token).
    /// </summary>
    /// <param name="account">Client account identifier.</param>
    /// <param name="subaccount">Client subaccount identifier.</param>
    /// <param name="razdel">Portfolio identifier.</param>
    /// <param name="control">Price control type identifier.</param>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="limit">Limit price.</param>
    /// <param name="trigger">Stop price.</param>
    /// <param name="alternative">Alternative limit price.</param>
    /// <param name="side">Trade direction: 1 for buy or -1 for sell.</param>
    /// <param name="quantity">Quantity in units.</param>
    /// <param name="comment">Order comment.</param>
    /// <param name="allowed">Allowed order parameters identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Order entry response entries.</returns>
    Task<IEntries> Entry(long account, long subaccount, long razdel, int control, long asset, double limit, double trigger, double alternative, int side, int quantity, string comment, long allowed, CancellationToken token = default);
}
