namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Sends routing request and returns its correlation id. Usage example: ICorrelationId id = await message.Send(token);.
/// </summary>
internal sealed class IncomingMessage : IIncomingMessage
{
    private readonly IRouting _routing;
    private readonly IRouterSocket _socket;

    /// <summary>
    /// Builds the incoming message sender. Usage example: var sender = new IncomingMessage(routing, socket).
    /// </summary>
    public IncomingMessage(IRouting routing, IRouterSocket socket)
    {
        ArgumentNullException.ThrowIfNull(routing);
        ArgumentNullException.ThrowIfNull(socket);
        _routing = routing;
        _socket = socket;
    }

    /// <summary>
    /// Sends the routing request and returns its correlation id. Usage example: await sender.Send(token).
    /// </summary>
    public async Task<ICorrelationId> Send(CancellationToken cancellationToken)
    {
        await _socket.Send(_routing.AsString(), cancellationToken);
        return new CorrelationId(_routing.Id());
    }
}
