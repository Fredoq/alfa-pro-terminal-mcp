using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Market board entries interface. Usage example: var entries = await boards.Entries(token);.
/// </summary>
public interface IMarketBoards
{
    /// <summary>
    /// Returns market board entries. Usage example: var entries = await boards.Entries(token);.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <summary>
/// Retrieves market board entries.
/// </summary>
/// <param name="token">Cancellation token to cancel the retrieval operation.</param>
/// <returns>An <see cref="IEntries"/> instance containing the market board entries.</returns>
    Task<IEntries> Entries(CancellationToken token = default);
}