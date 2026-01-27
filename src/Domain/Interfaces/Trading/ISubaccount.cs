using System.Threading;
using System.Threading.Tasks;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;

/// <summary>
/// Provides subaccount identifier resolution. Usage example: long id = await subaccount.Identifier(account, token).
/// </summary>
public interface ISubaccount
{
    /// <summary>
    /// Returns a subaccount identifier for the specified account. Usage example: long id = await subaccount.Identifier(account, token).
    /// </summary>
    /// <param name="account">Account identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Subaccount identifier.</returns>
    Task<long> Identifier(long account, CancellationToken token = default);
}
