namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Asset infos interface.
/// </summary>
public interface IAssetInfos
{
    /// <summary>
    /// Returns asset infos for given identifiers. Usage example: var assets = await infos.Info(ids, token);.
    /// </summary>
    /// <param name="ids">Collection of asset identifiers.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Asset infos entries.</returns>
    Task<IAssetInfosEntries> Info(IEnumerable<long> ids, CancellationToken cancellationToken = default);
}
