using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

/// <summary>
/// Represents routing request for order entry query. Usage example: var routing = new OrderEnterQueryRequest(payload); string message = routing.AsString();.
/// </summary>
public sealed record OrderEnterQueryRequest : IRouting
{
    private readonly string _id;
    private readonly string _command;
    private readonly string _channel;
    private readonly IPayload _payload;

    /// <summary>
    /// Creates order entry query request routing with generated correlation id. Usage example: var routing = new OrderEnterQueryRequest(payload).
    /// </summary>
    /// <param name="payload">Payload instance.</param>
    public OrderEnterQueryRequest(IPayload payload) : this(Guid.NewGuid().ToString(), "request", "#Order.Enter.Query", payload)
    {
    }

    /// <summary>
    /// Creates order entry query request routing with explicit values. Usage example: var routing = new OrderEnterQueryRequest(id, "request", "#Order.Enter.Query", payload).
    /// </summary>
    /// <param name="id">Correlation identifier.</param>
    /// <param name="command">Command name.</param>
    /// <param name="channel">Channel name.</param>
    /// <param name="payload">Payload instance.</param>
    private OrderEnterQueryRequest(string id, string command, string channel, IPayload payload)
    {
        _id = id;
        _command = command;
        _channel = channel;
        _payload = payload;
    }

    /// <summary>
    /// Serializes routing request into transport format. Usage example: string text = routing.AsString();.
    /// </summary>
    /// <returns>Serialized routing request.</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new { Id = _id, Command = _command, Channel = _channel, Payload = _payload.AsString() });

    /// <summary>
    /// Returns correlation id. Usage example: string id = routing.Id();.
    /// </summary>
    /// <returns>Correlation id value.</returns>
    public string Id() => _id;
}
