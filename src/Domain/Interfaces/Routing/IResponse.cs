using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

/// <summary>
/// Describes a stateless router response that can be validated and read. Usage example: bool accepted = response.Accepted(message, id); string payload = response.Payload(message);.
/// </summary>
public interface IResponse
{
    /// <summary>
    /// States whether the message matches the expected identifiers. Usage example: bool accepted = response.Accepted(message, id).
    /// </summary>
    bool Accepted(string message, ICorrelationId id);

    /// <summary>
    /// Returns the payload fragment of the message. Usage example: string payload = response.Payload(message).
    /// </summary>
    string Payload(string message);
}
