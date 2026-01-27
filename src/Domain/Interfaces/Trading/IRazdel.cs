using System.Threading;
using System.Threading.Tasks;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;

/// <summary>
/// Provides portfolio identifier resolution for subaccounts. Usage example: long id = await razdel.Identifier(account, subaccount, code, token).
/// </summary>
public interface IRazdel
{
    /// <summary>
    /// Returns a portfolio identifier for the specified account, subaccount, and code. Usage example: long id = await razdel.Identifier(account, subaccount, code, token).
    /// </summary>
    /// <param name="account">Account identifier.</param>
    /// <param name="subaccount">Subaccount identifier.</param>
    /// <param name="code">Portfolio code.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Portfolio identifier.</returns>
    Task<long> Identifier(long account, long subaccount, string code, CancellationToken token = default);
}
