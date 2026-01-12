using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Object group entries interface. Usage example: var entries = await groups.Entries(token);.
/// </summary>
public interface IObjectGroups
{
    /// <summary>
    /// Returns object group entries. Usage example: var entries = await groups.Entries(token);.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <summary>
/// Gets object group entries.
/// </summary>
/// <param name="token">Cancellation token to cancel the retrieval operation.</param>
/// <returns>An <see cref="IEntries"/> containing the object group entries.</returns>
    Task<IEntries> Entries(CancellationToken token = default);
}