using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Limit request entries interface. Usage example: var entries = await limits.Limit(1, 2, 3, 4, 5, 1, 120.5, 2, 3, token).
/// </summary>
public interface ILimits
{
    /// <summary>
    /// Returns limit entries for the specified parameters. Usage example: var entries = await limits.Limit(1, 2, 3, 4, 5, 1, 120.5, 2, 3, token).
    /// </summary>
    /// <param name="account">Client account identifier.</param>
    /// <param name="razdel">Portfolio identifier.</param>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="board">Market board identifier.</param>
    /// <param name="document">Document type identifier.</param>
    /// <param name="side">Trade direction: 1 for buy or -1 for sell.</param>
    /// <param name="price">Order price.</param>
    /// <param name="order">Order type identifier.</param>
    /// <param name="request">Requested limit type identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Limit entries.</returns>
    Task<IEntries> Limit(long account, long razdel, long asset, long board, long document, int side, double price, int order, int request, CancellationToken token = default);
}
