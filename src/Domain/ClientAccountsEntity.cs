namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain;

public sealed record ClientAccountsEntity : IPayload
{
    private readonly string _type;
    private readonly bool _init;

    public ClientAccountsEntity() : this("ClientAccountEntity", true)
    { }

    private ClientAccountsEntity(string type, bool init)
    {
        _type = type;
        _init = init;
    }

    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new
    {
        Type = _type,
        Init = _init
    });
}
