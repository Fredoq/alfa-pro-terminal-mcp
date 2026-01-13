using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Allowed order parameter entries interface. Usage example: var entries = await items.Entries(token).
/// </summary>
public interface IAllowedOrderParams
{
    /// <summary>
    /// Returns allowed order parameter entries. Usage example: var entries = await items.Entries(token).
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Allowed order parameter entries.</returns>
    Task<IEntries> Entries(CancellationToken token = default);
}
