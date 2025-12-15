using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Decorates responses to ignore heartbeat messages. Usage example: IResponse response = new HeartbeatResponse(original); bool accepted = response.Accepted(message, id);.
/// </summary>
internal sealed class HeartbeatResponse : IResponse
{
    private readonly IResponse _response;

    /// <summary>
    /// Creates a heartbeat-aware response. Usage example: IResponse response = new HeartbeatResponse(original).
    /// </summary>
    /// <param name="original">Original non-heartbeat response.</param>
    public HeartbeatResponse(IResponse original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _response = original;
    }

    /// <summary>
    /// States whether the message matches the expected identifiers. Usage example: bool accepted = response.Accepted(message, id).
    /// </summary>
    public bool Accepted(string message, ICorrelationId id)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        ArgumentNullException.ThrowIfNull(id);
        using JsonDocument document = JsonDocument.Parse(message);
        JsonElement root = document.RootElement;
        bool heartbeat = root.ValueKind == JsonValueKind.Object && root.TryGetProperty("heartbeat", out _);
        return !heartbeat && _response.Accepted(message, id);
    }

    /// <summary>
    /// Returns the payload fragment of the message. Usage example: string payload = response.Payload(message).
    /// </summary>
    public string Payload(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        return _response.Payload(message);
    }
}
