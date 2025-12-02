using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Reads router responses for archive queries. Usage example: var response = new ArchiveQueryResponse(message); if (response.Accepted(id)) { string payload = response.Payload(); }.
/// </summary>
internal sealed class ArchiveQueryResponse : IResponse
{
    private readonly string _message;

    public ArchiveQueryResponse(string message)
    {
        _message = message;
    }

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
        if (root.String("Channel") != "#Archive.Query")
        {
            return false;
        }
        return true;
    }

    public string Payload()
    {
        using JsonDocument document = JsonDocument.Parse(_message);
        JsonElement root = document.RootElement;
        return root.String("Payload").Trim('"');
    }
}
