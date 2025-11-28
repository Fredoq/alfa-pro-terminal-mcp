namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Reads router responses for data queries. Usage example: var response = new DataQueryResponse(message); if (response.Accepted(id)) { string payload = response.Payload(); }
/// </summary>
internal sealed class DataQueryResponse : IResponse
{
    private readonly string _payload;
    private readonly string _id;
    private readonly string _command;
    private readonly string _channel;

    /// <summary>
    /// Stores the raw message for parsing. Usage example: var response = new DataQueryResponse(message).
    /// </summary>
    public DataQueryResponse(string message) : this(JsonDocument.Parse(message))
    {
    }

    public DataQueryResponse(JsonDocument document) : this(document.RootElement)
    {

    }

    public DataQueryResponse(JsonElement element) : this(
        element.String("Id"),
        element.String("Command"),
        element.String("Channel"),
        element.String("Payload").Trim('"'))
    {

    }

    public DataQueryResponse(string id, string command, string channel, string payload)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentException.ThrowIfNullOrEmpty(command);
        ArgumentException.ThrowIfNullOrEmpty(channel);
        ArgumentException.ThrowIfNullOrEmpty(payload);
        _id = id;
        _command = command;
        _channel = channel;
        _payload = payload;
    }

    /// <summary>
    /// Reports whether the message belongs to the current query. Usage example: bool accepted = response.Accepted(id).
    /// </summary>
    public bool Accepted(ICorrelationId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        if (_id != id.Value())
        {
            return false;
        }
        if (_command != "response")
        {
            return false;
        }
        if (_channel != "#Data.Query")
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Provides the payload fragment when the message has been accepted. Usage example: string payload = response.Payload();
    /// </summary>
    public string Payload() => _payload;
}


internal static class JsonElementExtensions
{
    extension(JsonElement element)
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        public string String(string propertyName)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if (element.TryGetProperty(propertyName, out JsonElement value))
            {
                switch (value.ValueKind)
                {
                    case JsonValueKind.String:
                        return value.GetString() ?? throw new InvalidOperationException($"Property '{propertyName}' is null.");
                    case JsonValueKind.Null:
                        return string.Empty;
                }
            }
            throw new InvalidOperationException($"Property '{propertyName}' is missing or not a string.");
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        public int Number(string propertyName)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if (element.TryGetProperty(propertyName, out JsonElement value) && value.ValueKind == JsonValueKind.Number)
            {
                return value.GetInt32();
            }
            throw new InvalidOperationException($"Property '{propertyName}' is missing or not a number.");
        }
    }
}