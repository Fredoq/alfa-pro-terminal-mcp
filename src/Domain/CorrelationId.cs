namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Immutable correlation id value object. Usage example: var id = new CorrelationId("123"); string value = id.Value();.
/// </summary>
public sealed class CorrelationId : ICorrelationId
{
    private readonly string _value;

    /// <summary>
    /// Builds correlation id from text. Usage example: var id = new CorrelationId("abc").
    /// </summary>
    public CorrelationId(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        _value = value;
    }

    /// <summary>
    /// Returns id text. Usage example: string text = id.Value();.
    /// </summary>
    public string Value() => _value;
}
