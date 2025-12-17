using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds account balance JSON with field descriptions. Usage example: string json = new JsonAccountBalance(payload, accountId).Json().
/// </summary>
internal sealed class JsonAccountBalance : IAccountBalance
{
    private readonly string _payload;
    private readonly long _accountId;
    private readonly AccountBalanceSchema _schema;

    /// <summary>
    /// Creates parsing behavior for balance payload. Usage example: var balance = new JsonAccountBalance(payload, accountId).
    /// </summary>
    /// <param name="payload">Router payload.</param>
    /// <param name="accountId">Target account identifier.</param>
    public JsonAccountBalance(string payload, long accountId)
    {
        _payload = payload;
        _accountId = accountId;
        _schema = new AccountBalanceSchema();
    }

    /// <summary>
    /// Returns account balance JSON enriched with field descriptions. Usage example: string json = balance.Json().
    /// </summary>
    public string Json()
    {
        using JsonDocument document = JsonDocument.Parse(_payload);
        JsonElement root = document.RootElement;
        if (!root.TryGetProperty("Data", out JsonElement data))
        {
            throw new InvalidOperationException("Response data array is missing");
        }
        if (data.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Response data array is missing");
        }
        JsonArray list = [];
        foreach (JsonElement node in data.EnumerateArray())
        {
            long account = new JsonInteger(node, "IdAccount").Value();
            if (account != _accountId)
            {
                continue;
            }
            list.Add(_schema.Node(node));
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Account balance is missing");
        }
        return JsonSerializer.Serialize(list);
    }
}
