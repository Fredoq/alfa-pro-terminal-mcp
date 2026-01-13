using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Subaccount portfolio entries interface. Usage example: var entries = await items.Entries(token).
/// </summary>
public interface ISubAccountRazdels
{
    /// <summary>
    /// Returns subaccount portfolio entries. Usage example: var entries = await items.Entries(token).
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Subaccount portfolio entries.</returns>
    Task<IEntries> Entries(CancellationToken token = default);
}
