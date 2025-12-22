namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts.Filters;

/// <summary>
/// Provides ticker matching for filtering. Usage example: bool matched = tickers.Contains("AAA").
/// </summary>
public interface ITickers
{
    /// <summary>
    /// Checks whether the ticker matches the collection. Usage example: bool matched = tickers.Contains("AAA").
    /// </summary>
    bool Contains(string ticker);
}
