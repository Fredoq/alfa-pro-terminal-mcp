using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;
/// <summary>
/// Builds account entries from JSON payload. Usage example: string json = new JsonAccountsEntries(payload).Json().
/// </summary>
internal sealed class JsonAccountsEntries : IAccountsEntries
{
    private readonly string _payload;

    /// <summary>
    /// Creates parsing behavior for accounts. Usage example: var entries = new JsonAccountsEntries(payload).
    /// </summary>
    public JsonAccountsEntries(string payload)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
    }

    /// <summary>
    /// Returns accounts entries json with account id and IIA type. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        using JsonDocument doc = JsonDocument.Parse(_payload);
        JsonElement root = doc.RootElement;
        if (!root.TryGetProperty("Data", out JsonElement data))
        {
            throw new InvalidOperationException("Response data array is missing.");
        }
        if (data.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Response data array is missing");
        }
        JsonArray list = [];
        foreach (JsonElement node in data.EnumerateArray())
        {
            if (!node.TryGetProperty("IdAccount", out JsonElement id))
            {
                throw new InvalidOperationException("Account id is missing");
            }
            if (!node.TryGetProperty("IIAType", out JsonElement iia))
            {
                throw new InvalidOperationException("Account type is missing");
            }
            long account = id.ValueKind == JsonValueKind.Number ? id.GetInt64() : throw new InvalidOperationException("Account id is missing");
            int code = iia.ValueKind == JsonValueKind.Number ? iia.GetInt32() : throw new InvalidOperationException("Account type is missing");
            JsonObject entry = new()
            {
                ["AccountId"] = account,
                ["IIAType"] = code
            };
            list.Add(entry);
        }
        return JsonSerializer.Serialize(list);
    }
}
