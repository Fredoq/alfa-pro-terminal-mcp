using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Decorates responses to ignore heartbeat messages. Usage example: var response = new HeartbeatResponse(message, payload => new DataQueryResponse(payload)).
/// </summary>
internal sealed class HeartbeatResponse : IResponse
{
    private readonly string _message;
    private readonly IResponse _response;
    
    /// <summary>
    /// Creates a heartbeat-aware response. Usage example: var response = new HeartbeatResponse(message, factory).
    /// </summary>
    /// <param name="message">Raw router message.</param>
    /// <param name="original">Original non-heartbeat response.</param>
    public HeartbeatResponse(string message, IResponse original)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        ArgumentNullException.ThrowIfNull(original);
        _message = message;
        _response = original;
    }

    /// <summary>
    /// States whether the message matches the expected identifiers. Usage example: bool accepted = response.Accepted(id).
    /// </summary>
    public bool Accepted(ICorrelationId id)
    {
        using JsonDocument document = JsonDocument.Parse(_message);
        JsonElement root = document.RootElement;
        bool heartbeat = root.ValueKind == JsonValueKind.Object && root.TryGetProperty("heartbeat", out _);
        return !heartbeat && _response.Accepted(id);
    }

    /// <summary>
    /// Returns the payload fragment of the message. Usage example: string payload = response.Payload();
    /// </summary>
    public string Payload() => _response.Payload();
}
