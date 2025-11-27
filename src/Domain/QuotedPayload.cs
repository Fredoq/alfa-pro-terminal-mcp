namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

public record QuotedPayload : IPayload
{
    private readonly IPayload _payload;

    public QuotedPayload(IPayload payload)
    {
        _payload = payload;
    }
    public string AsString() => $"\"{_payload.AsString()}\"";
}
