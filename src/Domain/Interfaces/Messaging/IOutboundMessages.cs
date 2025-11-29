namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;

/// <summary>
/// Supplies outbound responses for previously sent requests. Usage example: IPayload payload = await outbound.Response(token);.
/// </summary>
public interface IOutboundMessages
{
    /// <summary>
    /// Returns the next response payload. Usage example: string payload = await outbound.NextMessage(token);.
    /// </summary>
    Task<string> NextMessage(CancellationToken cancellationToken);
}
