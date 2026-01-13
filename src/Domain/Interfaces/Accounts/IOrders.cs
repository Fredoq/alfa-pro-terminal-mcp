using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Orders interface. Usage example: var orders = await orders.Entries(123, token);.
/// </summary>
public interface IOrders
{
    /// <summary>
    /// Returns orders for the specified account. Usage example: var entries = await orders.Entries(123, token);.
    /// </summary>
    /// <param name="accountId">Target account identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order entries.</returns>
    Task<IEntries> Entries(long accountId, CancellationToken cancellationToken = default);
}
