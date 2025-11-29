using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

/// <summary>
/// Exercises CorrelationId value semantics. Usage example: executed by xUnit runner.
/// </summary>
public sealed class CorrelationIdTests
{
    /// <summary>
    /// Ensures that CorrelationId returns the assigned text. Usage example: new CorrelationId("abc").Value().
    /// </summary>
    [Fact(DisplayName = "CorrelationId returns assigned text")]
    public void Given_value_when_created_then_returns_same()
    {
        string text = $"ид-{Guid.NewGuid()}-φ";
        CorrelationId id = new(text);
        string value = id.Value();
        Assert.True(value == text, "CorrelationId does not preserve provided text");
    }

    /// <summary>
    /// Verifies that CorrelationId rejects empty text. Usage example: new CorrelationId(string.Empty).
    /// </summary>
    [Fact(DisplayName = "CorrelationId rejects empty text")]
    public void Given_empty_value_when_created_then_throws() => Assert.Throws<ArgumentException>(() => new CorrelationId(string.Empty));
}
