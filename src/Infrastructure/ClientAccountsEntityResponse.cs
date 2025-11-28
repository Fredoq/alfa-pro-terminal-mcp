namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

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
            DataQueryResponse response = new(message);
            if (!response.Accepted(id))
            {
                continue;
            }
            return response.Payload();
        }
        throw new InvalidOperationException("Response not received");
    }
}
