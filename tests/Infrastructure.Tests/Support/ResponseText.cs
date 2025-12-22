namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using System.Text.Json;

/// <summary>
/// Formats a router response payload into a serialized message. Usage example: string value = new ResponseText(id, payload, channel, command).Value();
/// </summary>
internal sealed class ResponseText : IText
{
    private readonly string id;
    private readonly string payload;
    private readonly string channel;
    private readonly string command;

    /// <summary>
    /// Creates a router response formatter. Usage example: new ResponseText(id, payload, channel, command).
    /// </summary>
    public ResponseText(string id, string payload, string channel, string command)
    {
        this.id = id;
        this.payload = payload;
        this.channel = channel;
        this.command = command;
    }

    /// <summary>
    /// Provides serialized router response text. Usage example: string value = text.Value();
    /// </summary>
    public string Value() => $"{{\"Id\":\"{id}\",\"Command\":\"{command}\",\"Channel\":\"{channel}\",\"Payload\":{JsonSerializer.Serialize(payload)}}}";
}

/// <summary>
/// Exposes a serialized router response text. Usage example: string value = text.Value();
/// </summary>
internal interface IText
{
    /// <summary>
    /// Provides serialized router response text. Usage example: string value = text.Value();
    /// </summary>
    string Value();
}
