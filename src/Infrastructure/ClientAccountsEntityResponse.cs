namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Retrieves a response for client accounts request. Usage example: IPayload payload = await response.Next(token);.
/// </summary>
internal sealed class ClientAccountsEntityResponse : IOutboundMessages
{
    private readonly IIncomingMessage _incoming;
    private readonly IRouterSocket _socket;

    /// <summary>
    /// Builds the response reader. Usage example: var response = new ClientAccountsEntityResponse(incoming, messages).
    /// </summary>
    public ClientAccountsEntityResponse(IIncomingMessage incoming, IRouterSocket socket)
    {
        ArgumentNullException.ThrowIfNull(incoming);
        ArgumentNullException.ThrowIfNull(socket);
        _incoming = incoming;
        _socket = socket;
    }

    /// <summary>
    /// Returns payload of the response. Usage example: IPayload payload = await Next(token);.
    /// </summary>
    public async Task<string> NextMessage(CancellationToken cancellationToken)
    {
        ICorrelationId id = await _incoming.Send(cancellationToken);
        await foreach (string message in _socket.Messages(cancellationToken))
        {
            RoutingResponse? response = JsonSerializer.Deserialize<RoutingResponse>(message);
            if (response is null)
            {
                throw new InvalidOperationException("Routing response is missing");
            }
            if (response.Id != id.Value())
            {
                continue;
            }
            if (response.Command != "response")
            {
                continue;
            }
            if (response.Channel != "#Data.Query")
            {
                continue;
            }
            if (string.IsNullOrWhiteSpace(response.Payload))
            {
                throw new InvalidOperationException("Routing payload is missing");
            }
            return response.Payload;
        }
        throw new InvalidOperationException("Response not received");
    }

    private sealed class RoutingResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
    }
}
