using System.Net.WebSockets;
using System.Security.Cryptography;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Configuration;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Extensions;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Validates service registrations and hosted service behaviors. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Ensures that Register wires AlfaProTerminal as terminal and hosted service. Usage example: services.Register(configuration).
    /// </summary>
    [Fact(DisplayName = "ServiceCollectionExtensions registers AlfaProTerminal and options")]
    public async Task Given_configuration_when_register_called_then_terminal_and_hosted_resolved()
    {
        int port = RandomNumberGenerator.GetInt32(10_000, 60_000);
        string endpoint = $"ws://127.0.0.1:{port}/router/";
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = endpoint };
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        ServiceCollection services = new();
        services.Register(configuration);
        await using ServiceProvider provider = services.BuildServiceProvider(true);
        ITerminal terminal = provider.GetRequiredService<ITerminal>();
        IHostedService hosted = provider.GetRequiredService<IHostedService>();
        AlfaProTerminal instance = provider.GetRequiredService<AlfaProTerminal>();
        TerminalOptions options = provider.GetRequiredService<IOptions<TerminalOptions>>().Value;
        bool registered = terminal is AlfaProTerminal && hosted is AlfaProTerminal && options.Endpoint == endpoint && instance is not null;
        Assert.True(registered, "ServiceCollectionExtensions does not wire AlfaProTerminal or options");
    }

    /// <summary>
    /// Confirms that AlfaProTerminal from DI establishes and closes router connection on start and stop. Usage example: await service.StartAsync(token).
    /// </summary>
    [Fact(DisplayName = "ServiceCollectionExtensions starts AlfaProTerminal connection")]
    public async Task Given_registered_terminal_when_started_then_connects_and_closes()
    {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));
        Uri http = new($"http://127.0.0.1:{RandomNumberGenerator.GetInt32(11_000, 58_000)}/router/");
        await using TestSocketHost host = new(http);
        await host.Start(source.Token);
        Dictionary<string, string?> settings = new() { ["Terminal:Endpoint"] = host.Endpoint().ToString() };
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        ServiceCollection services = new();
        services.Register(configuration);
        await using ServiceProvider provider = services.BuildServiceProvider(true);
        AlfaProTerminal terminal = provider.GetRequiredService<AlfaProTerminal>();
        await terminal.StartAsync(source.Token);
        WebSocket socket = await host.Take(source.Token);
        bool connected = socket.State == WebSocketState.Open;
        Task<WebSocketReceiveResult> acknowledgement = host.Acknowledge(source.Token);
        await terminal.StopAsync(source.Token);
        WebSocketReceiveResult result = await acknowledgement;
        bool closed = result.MessageType == WebSocketMessageType.Close;
        Assert.True(connected && closed, "ServiceCollectionExtensions does not start AlfaProTerminal connection");
    }
}
