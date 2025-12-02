using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;

/// <summary>
/// Represents routing request for archive query. Usage example: var routing = new ArchiveQueryRequest(payload); string message = routing.AsString();.
/// </summary>
public sealed record ArchiveQueryRequest : IRouting
{
    private readonly string _id;
    private readonly string _command;
    private readonly string _channel;
    private readonly IPayload _payload;

    public ArchiveQueryRequest(IPayload payload) : this(Guid.NewGuid().ToString(), "request", "#Archive.Query", payload)
    {
    }

    private ArchiveQueryRequest(string id, string command, string channel, IPayload payload)
    {
        _id = id;
        _command = command;
        _channel = channel;
        _payload = payload;
    }

    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new
    {
        Id = _id,
        Command = _command,
        Channel = _channel,
        Payload = _payload.AsString()
    });

    public string Id() => _id;
}
