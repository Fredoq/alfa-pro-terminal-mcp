using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;

/// <summary>
/// Supplies routing data for IncomingMessage tests. Usage example: new RoutingFake("text").
/// </summary>
internal sealed class RoutingFake : IRouting
{
    private readonly string payload;
    private readonly string identifier;

    /// <summary>
    /// Creates the routing fake with deterministic payload. Usage example: new RoutingFake("text").
    /// </summary>
    public RoutingFake(string payload)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload);
        this.payload = payload;
        identifier = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Returns correlation identifier. Usage example: routing.Id().
    /// </summary>
    public string Id() => identifier;

    /// <summary>
    /// Serializes routing payload. Usage example: routing.AsString().
    /// </summary>
    public string AsString() => payload;
}
