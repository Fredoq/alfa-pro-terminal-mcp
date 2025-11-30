using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Reads router responses for data queries. Usage example: var response = new DataQueryResponse(message); if (response.Accepted(id)) { string payload = response.Payload(); }
/// </summary>
internal sealed class DataQueryResponse : IResponse
{
    private readonly string _message;

    /// <summary>
    /// Stores the raw message for parsing. Usage example: var response = new DataQueryResponse(message).
    /// </summary>
    public DataQueryResponse(string message)
    {
        _message = message;
    }

    /// <summary>
    /// Reports whether the message belongs to the current query. Usage example: bool accepted = response.Accepted(id).
    /// </summary>
    public bool Accepted(ICorrelationId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        using JsonDocument document = JsonDocument.Parse(_message);
        JsonElement root = document.RootElement;
        if (root.String("Id") != id.Value())
        {
            return false;
        }
        if (root.String("Command") != "response")
        {
            return false;
        }
        if (root.String("Channel") != "#Data.Query")
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Provides the payload fragment when the message has been accepted. Usage example: string payload = response.Payload();
    /// </summary>
    public string Payload()
    {
        using JsonDocument document = JsonDocument.Parse(_message);
        JsonElement root = document.RootElement;
        return root.String("Payload").Trim('"');
    }
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
