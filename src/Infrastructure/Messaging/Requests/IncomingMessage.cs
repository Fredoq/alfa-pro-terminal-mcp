using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Requests;

/// <summary>
/// Sends routing request and returns its correlation id. Usage example: ICorrelationId id = await message.Send(token);.
/// </summary>
internal sealed partial class IncomingMessage : IIncomingMessage
{
    private readonly IRouting _routing;
    private readonly ITerminal _socket;
    private readonly ILogger _logger;

    /// <summary>
    /// Builds the incoming message sender. Usage example: var sender = new IncomingMessage(routing, socket).
    /// </summary>
    public IncomingMessage(IRouting routing, ITerminal socket, ILogger logger)
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
        Log(_logger, routing);
        await _socket.Send(routing, cancellationToken);
        return new CorrelationId(_routing.Id());
    }

    /// <summary>
    /// Logs sending of routing payload. Usage example: Log(logger, payload).
    /// </summary>
    /// <param name="logger">Target logger.</param>
    /// <param name="message">Serialized routing payload.</param>
    [LoggerMessage(Level = LogLevel.Debug, Message = "Sending routing message {Message}")]
    private static partial void Log(ILogger logger, string message);
}
