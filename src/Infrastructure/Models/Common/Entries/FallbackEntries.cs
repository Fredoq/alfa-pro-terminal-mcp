using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Chooses the first available entries from two sources. Usage example: string json = new FallbackEntries(primary, secondary).Json().
/// </summary>
internal sealed class FallbackEntries : IEntries
{
    private readonly IEntries _first;
    private readonly IEntries _second;

    /// <summary>
    /// Creates fallback behavior for entries. Usage example: var entries = new FallbackEntries(primary, secondary).
    /// </summary>
    /// <param name="first">Primary entries.</param>
    /// <param name="second">Secondary entries.</param>
    public FallbackEntries(IEntries first, IEntries second)
    {
        _first = first;
        _second = second;
    }

    /// <summary>
    /// Returns entries from the first source or falls back to the second. Usage example: string json = entries.Json().
    /// </summary>
    public string Json()
    {
        if (_first is null || _second is null)
        {
            throw new InvalidOperationException("Entries configuration is missing");
        }
        try
        {
            return _first.Json();
        }
        catch (MissingEntriesException)
        {
            return _second.Json();
        }
    }
}
