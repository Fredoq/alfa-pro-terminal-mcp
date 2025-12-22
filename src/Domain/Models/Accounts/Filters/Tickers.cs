namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts.Filters;

/// <summary>
/// Matches a normalized ticker set. Usage example: bool matched = new Tickers(source).Contains("AAA").
/// </summary>
public sealed class Tickers : ITickers
{
    private readonly IEnumerable<string> _items;

    /// <summary>
    /// Creates a ticker source wrapper. Usage example: var tickers = new Tickers(values).
    /// </summary>
    public Tickers(IEnumerable<string> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = items;
    }

    /// <summary>
    /// Checks whether the ticker matches the collection. Usage example: bool matched = tickers.Contains("AAA").
    /// </summary>
    public bool Contains(string ticker)
    {
        ArgumentException.ThrowIfNullOrEmpty(ticker);
        HashSet<string> set = new(StringComparer.OrdinalIgnoreCase);
        foreach (string item in _items)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException("Ticker value is invalid");
            }
            set.Add(item);
        }
        if (set.Count == 0)
        {
            throw new InvalidOperationException("Tickers list is empty");
        }
        return set.Contains(ticker);
    }
}
