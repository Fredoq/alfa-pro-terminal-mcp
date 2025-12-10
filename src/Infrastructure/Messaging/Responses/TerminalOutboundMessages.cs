using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Supplies outbound messages from the terminal stream. Usage example: var outbound = new TerminalOutboundMessages(incoming, socket, logger); string payload = await outbound.NextMessage(token);.
/// </summary>
internal sealed partial class TerminalOutboundMessages : IOutboundMessages
{
    private readonly IIncomingMessage _incoming;
    private readonly ITerminal _socket;
    private readonly ILogger _logger;

    /// <summary>
    /// Creates an outbound message reader. Usage example: var outbound = new TerminalOutboundMessages(incoming, socket, logger);.
    /// </summary>
    public TerminalOutboundMessages(IIncomingMessage incoming, ITerminal socket, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(incoming);
        ArgumentNullException.ThrowIfNull(socket);
        ArgumentNullException.ThrowIfNull(logger);
        _incoming = incoming;
        _socket = socket;
        _logger = logger;
    }

    /// <summary>
    /// Returns the next matching message payload. Usage example: string payload = await NextMessage(token);.
    /// </summary>
    public async Task<string> NextMessage(CancellationToken cancellationToken)
    {
        ICorrelationId id = await _incoming.Send(cancellationToken);
        await foreach (string message in _socket.Messages(cancellationToken))
        {
            Received(_logger, message);
            HeartbeatResponse response = new(message, new DataQueryResponse(message));
            if (!response.Accepted(id))
            {
                continue;
            }
            return response.Payload();
        }
        throw new InvalidOperationException("Response not received");
    }

    /// <summary>
    /// Logs received routing response. Usage example: Received(logger, payload).
    /// </summary>
    /// <param name="logger">Target logger.</param>
    /// <param name="message">Response payload.</param>
    [LoggerMessage(Level = LogLevel.Debug, Message = "Received routing message {Message}")]
    private static partial void Received(ILogger logger, string message);
}
