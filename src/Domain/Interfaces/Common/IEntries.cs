namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

/// <summary>
/// Represents entries payload. Usage example: string json = entries.Json();.
/// </summary>
public interface IEntries
{
    /// <summary>
    /// Returns entries as JSON. Usage example: string json = entries.Json();.
    /// </summary>
    string Json();
}
