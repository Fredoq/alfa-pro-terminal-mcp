using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Messaging;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Supplies correlation identifier for outbound requests. Usage example: new IncomingStub(id).
/// </summary>
internal sealed class IncomingStub : IIncomingMessage
{
    private readonly ICorrelationId correlation;

    /// <summary>
    /// Creates the stub with fixed identifier. Usage example: new IncomingStub(id).
    /// </summary>
    public IncomingStub(string id)
    {
        correlation = new CorrelationId(id);
    }

    /// <summary>
    /// Returns the correlation identifier. Usage example: await stub.Send(token).
    /// </summary>
    public Task<ICorrelationId> Send(CancellationToken cancellationToken) => Task.FromResult(correlation);
}
