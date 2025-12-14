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
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;
    private readonly string _message;
    private readonly ICorrelationId _id;

    /// <summary>
    /// Builds the incoming message sender. Usage example: var sender = new IncomingMessage(routing, socket).
    /// </summary>
    public IncomingMessage(IRouting routing, ITerminal terminal, ILogger logger)
        : this(logger, routing.AsString(), routing.Id(), terminal)
    {
    }

    public IncomingMessage(ILogger logger, string message, string id, ITerminal terminal)
        : this(logger, message, new CorrelationId(id), terminal)
    {
    }

    public IncomingMessage(ILogger logger, string message, ICorrelationId id, ITerminal terminal)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(terminal);
        _message = message;
        _id = id;
        _terminal = terminal;
        _logger = logger;
    }

    /// <summary>
    /// Sends the routing request and returns its correlation id. Usage example: await sender.Send(token).
    /// </summary>
    public async Task<ICorrelationId> Send(CancellationToken cancellationToken)
    {
        Log(_logger, _message);
        await _terminal.Send(_message, cancellationToken);
        return _id;
    }

    /// <summary>
    /// Logs sending of routing payload. Usage example: Log(logger, payload).
    /// </summary>
    /// <param name="logger">Target logger.</param>
    /// <param name="message">Serialized routing payload.</param>
    [LoggerMessage(Level = LogLevel.Debug, Message = "Sending routing message {Message}")]
    private static partial void Log(ILogger logger, string message);
}
