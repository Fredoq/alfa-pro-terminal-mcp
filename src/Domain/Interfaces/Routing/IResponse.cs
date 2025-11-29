using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

/// <summary>
/// Describes a router response that can be validated and read. Usage example: bool accepted = response.Accepted(id).
/// </summary>
public interface IResponse
{
    /// <summary>
    /// States whether the message matches the expected identifiers. Usage example: bool accepted = response.Accepted(id).
    /// </summary>
    bool Accepted(ICorrelationId id);

    /// <summary>
    /// Returns the payload fragment of the message. Usage example: string payload = response.Payload();
    /// </summary>
    string Payload();
}
