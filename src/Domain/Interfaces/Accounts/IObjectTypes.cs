using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Object type entries interface. Usage example: var entries = await types.Entries(token);.
/// </summary>
public interface IObjectTypes
{
    /// <summary>
    /// Returns object type entries. Usage example: var entries = await types.Entries(token);.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <summary>
/// Gets object type entries.
/// </summary>
/// <param name="token">Cancellation token that can be used to cancel the operation.</param>
/// <returns>An <see cref="IEntries"/> instance representing the object type entries.</returns>
    Task<IEntries> Entries(CancellationToken token = default);
}