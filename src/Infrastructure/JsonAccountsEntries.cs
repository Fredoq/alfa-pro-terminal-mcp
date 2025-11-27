namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

using System.Collections.Immutable;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

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
        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new ArgumentException("Payload is empty", nameof(payload));
        }
        _payload = payload;
    }

    /// <summary>
    /// Parses payload into immutable account list. Usage example: IImmutableList&lt;IAccount&gt; list = await entries.Entries(token);.
    /// </summary>
    public Task<IImmutableList<IAccount>> Entries()
    {
        Data? data = JsonSerializer.Deserialize<Data>(_payload);
        if (data is null)
        {
            throw new InvalidOperationException("Accounts data is missing");
        }
        if (data.Type != "ClientAccountEntity")
        {
            throw new InvalidOperationException("Unexpected accounts entity");
        }
        if (data.Updated is null)
        {
            throw new InvalidOperationException("Accounts are missing");
        }
        ImmutableList<IAccount>.Builder builder = ImmutableList.CreateBuilder<IAccount>();
        foreach (AccountItem item in data.Updated)
        {
            builder.Add(new Account(item.IdAccount, item.IIAType));
        }
        return Task.FromResult<IImmutableList<IAccount>>(builder.ToImmutable());
    }

    /// <summary>
    /// Represents accounts data envelope from payload. Usage example: deserialized inside Entries.
    /// </summary>
    private sealed class Data
    {
        public string Type { get; set; } = string.Empty;
        public IReadOnlyCollection<AccountItem>? Updated { get; set; } = Array.Empty<AccountItem>();
    }

    /// <summary>
    /// Represents account row in updated collection. Usage example: deserialized item.
    /// </summary>
    private sealed class AccountItem
    {
        public long IdAccount { get; set; } = -1;
        public int IIAType { get; set; } = -1;
    }
}
