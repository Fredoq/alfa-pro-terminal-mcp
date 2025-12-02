using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Retrieves archive query responses through the router. Usage example: string payload = await new ArchiveQueryMessages(incoming, socket, logger).NextMessage(token);.
/// </summary>
internal sealed partial class ArchiveQueryMessages : IOutboundMessages
{
    private readonly IIncomingMessage _incoming;
    private readonly IRouterSocket _socket;
    private readonly ILogger _logger;

    public ArchiveQueryMessages(IIncomingMessage incoming, IRouterSocket socket, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(incoming);
        ArgumentNullException.ThrowIfNull(socket);
        ArgumentNullException.ThrowIfNull(logger);
        _incoming = incoming;
        _socket = socket;
        _logger = logger;
    }

    public async Task<string> NextMessage(CancellationToken cancellationToken)
    {
        ICorrelationId id = await _incoming.Send(cancellationToken);
        await foreach (string message in _socket.Messages(cancellationToken))
        {
            Received(_logger, message);
            HeartbeatResponse response = new(message, new ArchiveQueryResponse(message));
            if (!response.Accepted(id))
            {
                continue;
            }
            return response.Payload();
        }
        throw new InvalidOperationException("Response not received");
    }

    [LoggerMessage(Level = LogLevel.Debug, Message = "Received routing message {Message}")]
    private static partial void Received(ILogger logger, string message);
}
