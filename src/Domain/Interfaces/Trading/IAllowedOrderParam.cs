using System.Threading;
using System.Threading.Tasks;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;

/// <summary>
/// Provides allowed order parameter identifier resolution. Usage example: long id = await param.Identifier(group, market, price, token).
/// </summary>
public interface IAllowedOrderParam
{
    /// <summary>
    /// Returns allowed order parameter identifier for the provided scope. Usage example: long id = await param.Identifier(group, market, price, token).
    /// </summary>
    /// <param name="group">Object group identifier.</param>
    /// <param name="market">Market board identifier.</param>
    /// <param name="price">Limit price.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Allowed order parameter identifier.</returns>
    Task<long> Identifier(long group, long market, double price, CancellationToken token = default);
}
