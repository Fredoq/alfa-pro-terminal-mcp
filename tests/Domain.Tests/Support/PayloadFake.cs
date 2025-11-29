using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests.Support;

using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Supplies deterministic payload text for routing tests. Usage example: new PayloadFake("abc").AsString().
/// </summary>
internal sealed class PayloadFake : IPayload
{
    private readonly string value;

    /// <summary>
    /// Creates the fake payload with provided content. Usage example: new PayloadFake("abc").
    /// </summary>
    public PayloadFake(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);
        this.value = value;
    }

    /// <summary>
    /// Returns serialized payload text. Usage example: payload.AsString().
    /// </summary>
    public string AsString() => JsonSerializer.Serialize(new { Content = value });
}
