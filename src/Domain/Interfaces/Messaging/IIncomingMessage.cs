using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;

/// <summary>
/// Sends routing requests and exposes their correlation id. Usage example: ICorrelationId id = await message.Send(token);.
/// </summary>
public interface IIncomingMessage
{
    /// <summary>
    /// Sends the message and returns its correlation id. Usage example: ICorrelationId id = await message.Send(token);.
    /// </summary>
    Task<ICorrelationId> Send(CancellationToken cancellationToken);
}
