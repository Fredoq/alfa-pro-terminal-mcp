using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Archive;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Builds archive candles JSON with field descriptions. Usage example: string json = new JsonArchiveEntries(payload).Json().
/// </summary>
internal sealed class JsonArchiveEntries : IArchiveEntries
{
    private readonly string _payload;
    private readonly OhlcvSchema _ohlcv;
    private readonly MpvSchema _mpv;

    public JsonArchiveEntries(string payload)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _payload = payload;
        _ohlcv = new OhlcvSchema();
        _mpv = new MpvSchema();
    }

    public string Json()
    {
        using JsonDocument document = JsonDocument.Parse(_payload);
        JsonElement root = document.RootElement;
        JsonArray list = [];
        if (root.TryGetProperty("OHLCV", out JsonElement ohlcv) && ohlcv.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement node in ohlcv.EnumerateArray())
            {
                list.Add(_ohlcv.Node(node));
            }
        }
        else if (root.TryGetProperty("MPV", out JsonElement mpv) && mpv.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement node in mpv.EnumerateArray())
            {
                list.Add(_mpv.Node(node));
            }
        }
        else
        {
            throw new InvalidOperationException("Archive candles are missing");
        }
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Archive candles are missing");
        }
        return JsonSerializer.Serialize(list);
    }
}
