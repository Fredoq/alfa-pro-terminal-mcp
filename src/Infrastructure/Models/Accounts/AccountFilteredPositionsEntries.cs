using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Filters positions JSON by account identifier. Usage example: string json = new AccountFilteredPositionsEntries(entries, account).Json().
/// </summary>
internal sealed class AccountFilteredPositionsEntries : IPositionsEntries
{
    private readonly IPositionsEntries _entries;
    private readonly long _account;

    /// <summary>
    /// Creates account filtering behavior over positions JSON. Usage example: var entries = new AccountFilteredPositionsEntries(inner, account).
    /// </summary>
    /// <param name="entries">Source positions entries.</param>
    /// <param name="account">Target account identifier.</param>
    public AccountFilteredPositionsEntries(IPositionsEntries entries, long account)
    {
        _entries = entries ?? throw new ArgumentNullException(nameof(entries));
        _account = account;
    }

    /// <summary>
    /// Returns positions JSON filtered by account. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        string json = _entries.Json();
        JsonDocument document = JsonDocument.Parse(json);
        using (document)
        {
            JsonElement root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Positions array is missing");
            }
            JsonArray list = [];
            foreach (JsonElement item in root.EnumerateArray())
            {
                long account = new JsonInteger(item, "IdAccount").Value();
                if (account != _account)
                {
                    continue;
                }
                JsonNode value = JsonNode.Parse(item.GetRawText()) ?? throw new InvalidOperationException("Position node is missing");
                list.Add(value);
            }
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Account positions are missing");
            }
            return JsonSerializer.Serialize(list);
        }
    }
}
