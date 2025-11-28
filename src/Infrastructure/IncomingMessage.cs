namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// Sends routing request and returns its correlation id. Usage example: ICorrelationId id = await message.Send(token);.
/// </summary>
internal sealed class IncomingMessage : IIncomingMessage
{
    private readonly IRouting _routing;
    private readonly IRouterSocket _socket;
    private readonly ILogger _logger;

    /// <summary>
    /// Builds the incoming message sender. Usage example: var sender = new IncomingMessage(routing, socket).
    /// </summary>
    public IncomingMessage(IRouting routing, IRouterSocket socket, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(routing);
        ArgumentNullException.ThrowIfNull(socket);
        ArgumentNullException.ThrowIfNull(logger);
        _routing = routing;
        _socket = socket;
        _logger = logger;
    }

    /// <summary>
    /// Sends the routing request and returns its correlation id. Usage example: await sender.Send(token).
    /// </summary>
    public async Task<ICorrelationId> Send(CancellationToken cancellationToken)
    {
        string routing = _routing.AsString();
#pragma warning disable CA1848 // Use the LoggerMessage delegates
        _logger.LogDebug("Sending message with message {Message}", routing);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        await _socket.Send(routing, cancellationToken);
        return new CorrelationId(_routing.Id());
    }
}
