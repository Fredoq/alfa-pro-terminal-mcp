using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Returns original positions JSON array from a router payload. Usage example: string json = new AccountPositionsEntries(payload).Json().
/// </summary>
internal sealed class AccountPositionsEntries : IPositionsEntries
{
    private readonly string _payload;

    /// <summary>
    /// Creates positions behavior based on the router payload. Usage example: var entries = new AccountPositionsEntries(payload).
    /// </summary>
    /// <param name="payload">Router payload.</param>
    public AccountPositionsEntries(string payload) => _payload = string.IsNullOrEmpty(payload) ? throw new ArgumentException("Payload is empty", nameof(payload)) : payload;

    /// <summary>
    /// Returns original positions JSON array. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        JsonDocument document = JsonDocument.Parse(_payload);
        using (document)
        {
            JsonElement root = document.RootElement;
            if (!root.TryGetProperty("Data", out JsonElement data) || data.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Response data array is missing");
            }
            if (data.GetArrayLength() == 0)
            {
                throw new InvalidOperationException("Positions are missing");
            }
            return data.GetRawText();
        }
    }
}
