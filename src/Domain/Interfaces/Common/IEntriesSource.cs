using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

/// <summary>
/// Entries source contract. Usage example: IEntries entries = await source.Entries(payload, token).
/// </summary>
public interface IEntriesSource
{
    /// <summary>
    /// Returns entries for the specified payload. Usage example: IEntries entries = await source.Entries(payload, token).
    /// </summary>
    /// <param name="payload">Request payload.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Entries result.</returns>
    Task<IEntries> Entries(IPayload payload, CancellationToken token = default);
}
