using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

/// <summary>
/// Wraps a raw JSON payload string. Usage example: IPayload payload = new TextPayload(text).
/// </summary>
/// <param name="Text">Serialized JSON payload.</param>
public sealed record TextPayload(string Text) : IPayload
{
    /// <summary>
    /// Returns the payload text. Usage example: string json = payload.AsString().
    /// </summary>
    /// <returns>Serialized payload string.</returns>
    public string AsString() => Text;
}
