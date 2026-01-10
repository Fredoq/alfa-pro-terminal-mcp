using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Fin info params entries interface. Usage example: var entries = await info.Entries(123, token);.
/// </summary>
public interface IFinInfoParams
{
    /// <summary>
    /// Returns fin info params entries for a financial instrument. Usage example: var entries = await info.Entries(123, token);.
    /// </summary>
    /// <param name="id">Financial instrument identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Fin info params entries.</returns>
    Task<IEntries> Entries(long id, CancellationToken token = default);
}
