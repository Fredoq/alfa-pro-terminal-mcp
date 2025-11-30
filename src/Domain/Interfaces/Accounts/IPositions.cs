namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Positions interface.
/// </summary>
public interface IPositions
{
    /// <summary>
    /// Returns positions for the specified account. Usage example: var positions = await positions.Positions(123, token);.
    /// </summary>
    /// <param name="accountId">Target account identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Positions entries.</returns>
    Task<IPositionsEntries> Entries(long accountId, CancellationToken cancellationToken = default);
}
