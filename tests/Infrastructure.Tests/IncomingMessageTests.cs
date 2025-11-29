using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Validates IncomingMessage behavior when sending routing payloads. Usage example: executed by xUnit runner.
/// </summary>
public sealed class IncomingMessageTests
{
    /// <summary>
    /// Ensures that IncomingMessage sends payload and returns correlation id. Usage example: await message.Send(token).
    /// </summary>
    [Fact(DisplayName = "IncomingMessage sends routing payload and returns correlation id")]
    public async Task Given_routing_when_sent_then_payload_and_id_returned()
    {
        string text = $"payload-{Guid.NewGuid()}-ю";
        RoutingFake routing = new(text);
        await using RouterSocketSpy socket = new();
        LoggerFake logger = new();
        Messaging.Requests.IncomingMessage message = new(routing, socket, logger);
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
        ICorrelationId id = await message.Send(source.Token);
        bool sent = socket.Payload == routing.AsString() && id.Value() == routing.Id();
        Assert.True(sent, "IncomingMessage does not send payload or return correlation");
    }
}
