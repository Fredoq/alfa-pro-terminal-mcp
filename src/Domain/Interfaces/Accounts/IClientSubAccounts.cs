using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Client subaccount entries interface. Usage example: var entries = await items.Entries(token).
/// </summary>
public interface IClientSubAccounts
{
    /// <summary>
    /// Returns client subaccount entries. Usage example: var entries = await items.Entries(token).
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Client subaccount entries.</returns>
    Task<IEntries> Entries(CancellationToken token = default);
}
