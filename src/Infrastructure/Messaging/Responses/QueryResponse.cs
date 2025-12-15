using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Reads router responses for a specific query channel.
/// Usage example: IResponse response = new QueryResponse("#Data.Query"); bool accepted = response.Accepted(message, id);.
/// </summary>
internal sealed class QueryResponse : IResponse
{
    private readonly string _channel;

    /// <summary>
    /// Creates a response reader bound to a channel.
    /// Usage example: IResponse response = new QueryResponse("#Archive.Query");.
    /// </summary>
    /// <param name="channel">Router channel name</param>
    public QueryResponse(string channel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(channel);
        _channel = channel;
    }

    /// <summary>
    /// Reports whether the message belongs to the current query.
    /// Usage example: bool accepted = response.Accepted(message, id).
    /// </summary>
    public bool Accepted(string message, ICorrelationId id)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        ArgumentNullException.ThrowIfNull(id);
        using JsonDocument document = JsonDocument.Parse(message);
        JsonElement root = document.RootElement;
        if (root.String("Id") != id.Value())
        {
            return false;
        }
        if (root.String("Command") != "response")
        {
            return false;
        }
        if (root.String("Channel") != _channel)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Provides the payload fragment when the message has been accepted.
    /// Usage example: string payload = response.Payload(message).
    /// </summary>
    public string Payload(string message)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        using JsonDocument document = JsonDocument.Parse(message);
        JsonElement root = document.RootElement;
        return root.String("Payload").Trim('"');
    }
}
