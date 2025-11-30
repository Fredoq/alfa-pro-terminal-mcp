namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

/// <summary>
/// Account balance interface.
/// </summary>
public interface IAccountBalance
{
    /// <summary>
    /// Returns an account balance serialized with field descriptions. Usage example: string json = balance.Json();.
    /// </summary>
    /// <returns>Account balance in JSON format.</returns>
    string Json();
}
