using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Narrows asset payloads to specified tickers. Usage example: bool matched = new AssetTickersScope(tickers).Filtered(node);.
/// </summary>
internal sealed class AssetTickersScope : IAssetFilter
{
    private readonly HashSet<string> _tickers;

    /// <summary>
    /// Creates filter limited to provided tickers. Usage example: new AssetTickersScope(tickers).
    /// </summary>
    public AssetTickersScope(IEnumerable<string> tickers)
    {
        ArgumentNullException.ThrowIfNull(tickers);
        _tickers = Build(tickers);
    }

    /// <summary>
    /// Checks whether payload matches ticker list. Usage example: scope.Filtered(node).
    /// </summary>
    public bool Filtered(JsonElement node)
    {
        string value = Ticker(node);
        return _tickers.Contains(value);
    }

    /// <summary>
    /// Builds normalized ticker set. Usage example: HashSet&lt;string&gt; set = Build(tickers);.
    /// </summary>
    private static HashSet<string> Build(IEnumerable<string> tickers)
    {
        HashSet<string> set = new(StringComparer.OrdinalIgnoreCase);
        foreach (string ticker in tickers)
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                throw new ArgumentException("Ticker value is invalid");
            }
            set.Add(ticker);
        }
        if (set.Count == 0)
        {
            throw new InvalidOperationException("Tickers list is empty");
        }
        return set;
    }

    /// <summary>
    /// Extracts ticker from payload. Usage example: string ticker = Ticker(node);.
    /// </summary>
    private static string Ticker(JsonElement node)
    {
        if (!node.TryGetProperty("Ticker", out JsonElement value))
        {
            throw new InvalidOperationException("Ticker is missing");
        }
        if (value.ValueKind == JsonValueKind.String)
        {
            return value.GetString() ?? string.Empty;
        }
        if (value.ValueKind == JsonValueKind.Null)
        {
            return string.Empty;
        }
        throw new InvalidOperationException("Ticker is missing");
    }
}
