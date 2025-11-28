using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;
/// <summary>
/// Builds account entries from JSON payload. Usage example: var entries = new JsonAccountsEntries(payload, options); IImmutableList&lt;IAccount&gt; list = entries.Entries();.
/// </summary>
internal sealed class JsonAccountsEntries : IAccountsEntries
{
    private readonly string _payload;

    /// <summary>
    /// Creates parsing behavior for accounts. Usage example: var entries = new JsonAccountsEntries(payload, options).
    /// </summary>
    public JsonAccountsEntries(string payload)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
    }

    public string Json() => throw new NotImplementedException();
}
