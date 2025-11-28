namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Accounts interface.
/// </summary>
public interface IAccounts
{
    /// <summary>
    /// Returns the collection of client accounts.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Collection of client accounts.</returns>
    Task<IAccountsEntries> Entries(CancellationToken cancellationToken = default);
}
