namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Balances interface.
/// </summary>
public interface IBalances
{
    /// <summary>
    /// Returns balance entries for the specified account. Usage example: var balance = await balances.Balance(123, token);.
    /// </summary>
    /// <param name="accountId">Target account identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account balance entries.</returns>
    Task<IAccountBalance> Balance(long accountId, CancellationToken cancellationToken = default);
}
