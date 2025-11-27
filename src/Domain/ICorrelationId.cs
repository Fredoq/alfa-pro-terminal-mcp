namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Represents a correlation identifier for routing requests. Usage example: string value = id.Value();.
/// </summary>
public interface ICorrelationId
{
    /// <summary>
    /// Returns identifier text. Usage example: string value = id.Value();.
    /// </summary>
    string Value();
}
