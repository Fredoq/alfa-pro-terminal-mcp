using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;

/// <summary>
/// Narrows asset payloads to specified tickers. Usage example: bool matched = new AssetTickersScope(tickers).Filtered(node);.
/// </summary>
internal sealed class AssetTickersScope : IEntriesFilter
{
    private readonly ITickers _tickers;

    /// <summary>
    /// Creates filter limited to provided tickers. Usage example: new AssetTickersScope(tickers).
    /// </summary>
    public AssetTickersScope(IEnumerable<string> tickers)
        : this(new Tickers(tickers))
    {
    }

    /// <summary>
    /// Creates filter using a ticker collection. Usage example: new AssetTickersScope(tickers).
    /// </summary>
    public AssetTickersScope(ITickers tickers)
    {
        ArgumentNullException.ThrowIfNull(tickers);
        _tickers = tickers;
    }

    /// <summary>
    /// Checks whether payload matches ticker list. Usage example: scope.Filtered(node).
    /// </summary>
    public bool Filtered(JsonObject node)
    {
        string value = new JsonString(node, "Ticker").Value();
        return _tickers.Contains(value);
    }
}
