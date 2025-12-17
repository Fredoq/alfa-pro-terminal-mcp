using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Builds asset infos from JSON payload. Usage example: string json = new JsonAssetInfosEntries(payload, filter).Json().
/// </summary>
internal sealed class JsonAssetInfosEntries : IAssetInfosEntries
{
    private readonly string _payload;
    private readonly IAssetFilter _filter;
    private readonly AssetInfoSchema _schema;

    /// <summary>
    /// Creates parsing behavior for asset infos. Usage example: var infos = new JsonAssetInfosEntries(payload, filter).
    /// </summary>
    public JsonAssetInfosEntries(string payload, IAssetFilter filter)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
        _schema = new AssetInfoSchema();
    }

    /// <summary>
    /// Returns asset infos json filtered by identifiers with descriptions. Usage example: string json = infos.Json().
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
            if (!_filter.Filtered(node))
            {
                continue;
            }
            list.Add(_schema.Node(node));
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Asset infos are missing");
        }
        return JsonSerializer.Serialize(list);
    }
}
