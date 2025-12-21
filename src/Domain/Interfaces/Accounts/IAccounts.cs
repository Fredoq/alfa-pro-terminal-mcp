using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

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
    Task<IEntries> Entries(CancellationToken cancellationToken = default);
}
