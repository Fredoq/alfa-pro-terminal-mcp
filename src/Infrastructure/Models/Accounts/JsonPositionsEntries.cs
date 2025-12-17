using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds position entries from JSON payload. Usage example: string json = new JsonPositionsEntries(payload, accountId).Json().
/// </summary>
internal sealed class JsonPositionsEntries : IPositionsEntries
{
    private readonly string _payload;
    private readonly long _accountId;
    private readonly PositionSchema _schema;

    /// <summary>
    /// Creates parsing behavior for positions. Usage example: var entries = new JsonPositionsEntries(payload, accountId).
    /// </summary>
    public JsonPositionsEntries(string payload, long accountId)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
        _accountId = accountId;
        _schema = new PositionSchema();
    }

    /// <summary>
    /// Returns positions entries json filtered by account with descriptions. Usage example: string json = entries.Json().
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
            throw new InvalidOperationException("Account positions are missing");
        }
        return JsonSerializer.Serialize(list);
    }
}
