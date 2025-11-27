namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Describes available terminal operations. Usage example: IAccounts accounts = await terminal.Accounts(token);.
/// </summary>
public interface ITerminal
{
    /// <summary>
    /// Returns the accounts known to the terminal. Usage example: IAccounts accounts = await terminal.Accounts(token);.
    /// </summary>
    Task<IAccounts> Accounts(CancellationToken cancellationToken);
}
