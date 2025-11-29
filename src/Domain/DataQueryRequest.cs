namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

public sealed record DataQueryRequest : IRouting
{
    private readonly string _id;
    private readonly string _command;
    private readonly string _channel;
    private readonly IPayload _payload;

    public DataQueryRequest(IPayload payload) : this(Guid.NewGuid().ToString(), "request", "#Data.Query", payload)
    {
    }

    private DataQueryRequest(string id, string command, string channel, IPayload payload)
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
