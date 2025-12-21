using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;

/// <summary>
/// Filters entries by account identifier. Usage example: bool matched = new AccountScope(account).Filtered(node).
/// </summary>
internal sealed class AccountScope : IEntriesFilter
{
    private readonly long _account;
    private readonly string _name;

    /// <summary>
    /// Creates an account scope with the default IdAccount field. Usage example: var scope = new AccountScope(account).
    /// </summary>
    /// <param name="account">Target account identifier.</param>
    public AccountScope(long account) : this(account, "IdAccount")
    {
    }

    /// <summary>
    /// Creates an account scope with a custom field name. Usage example: var scope = new AccountScope(account, "IdAccount").
    /// </summary>
    /// <param name="account">Target account identifier.</param>
    /// <param name="name">Account field name.</param>
    public AccountScope(long account, string name)
    {
        _account = account;
        _name = name;
    }

    /// <summary>
    /// Determines whether the node matches the account. Usage example: bool matched = scope.Filtered(node).
    /// </summary>
    /// <param name="node">Account payload element.</param>
    public bool Filtered(JsonElement node)
    {
        if (string.IsNullOrEmpty(_name))
        {
            throw new InvalidOperationException("Account field name is missing");
        }
        long account = new JsonInteger(node, _name).Value();
        return account == _account;
    }
}
