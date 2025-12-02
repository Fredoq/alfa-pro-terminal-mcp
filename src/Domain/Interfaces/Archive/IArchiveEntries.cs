namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Archive;

/// <summary>
/// Archive candles collection contract.
/// </summary>
public interface IArchiveEntries
{
    /// <summary>
    /// Returns archive candles serialized with descriptions. Usage example: string json = entries.Json();.
    /// </summary>
    /// <returns>Serialized candles payload.</returns>
    string Json();
}
