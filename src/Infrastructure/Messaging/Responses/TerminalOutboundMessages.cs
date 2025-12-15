using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Supplies outbound messages from the terminal stream. Usage example: var outbound = new TerminalOutboundMessages(incoming, socket, logger, response); string payload = await outbound.NextMessage(token);.
/// </summary>
internal sealed class TerminalOutboundMessages : IOutboundMessages
{
    private readonly IIncomingMessage _incoming;
    private readonly ITerminal _socket;
    private readonly ILogger _logger;
    private readonly IResponse _response;

    /// <summary>
    /// Creates an outbound message reader with a custom response matcher. Usage example: var outbound = new TerminalOutboundMessages(incoming, socket, logger, response);.
    /// </summary>
    public TerminalOutboundMessages(IIncomingMessage incoming, ITerminal socket, ILogger logger, IResponse response)
    {
        ArgumentNullException.ThrowIfNull(incoming);
        ArgumentNullException.ThrowIfNull(socket);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(response);
        _incoming = incoming;
        _socket = socket;
        _logger = logger;
        _response = response;
    }

    /// <summary>
    /// Returns the next matching message payload. Usage example: string payload = await NextMessage(token);.
    /// </summary>
    public async Task<string> NextMessage(CancellationToken cancellationToken)
    {
        ICorrelationId id = await _incoming.Send(cancellationToken);
        await foreach (string message in _socket.Messages(cancellationToken))
        {
            _logger.LogDebug("Received routing message {Message}", message);
            if (!_response.Accepted(message, id))
            {
                continue;
            }
            return _response.Payload(message);
        }
        throw new InvalidOperationException("Response not received");
    }
}
