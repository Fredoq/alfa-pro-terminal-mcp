using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Configuration;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Extensions;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Validates service registrations and hosted service behaviors. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Ensures that Register wires router socket and options. Usage example: services.Register(configuration).
    /// </summary>
    [Fact(DisplayName = "ServiceCollectionExtensions registers router socket and options")]
    public async Task Given_configuration_when_register_called_then_services_resolved()
    {
        int port = RandomNumberGenerator.GetInt32(10_000, 60_000);
        string endpoint = $"ws://127.0.0.1:{port}/router/";
        Dictionary<string, string?> settings = new() { ["Router:Endpoint"] = endpoint };
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        ServiceCollection services = new();
        services.Register(configuration);
        await using ServiceProvider provider = services.BuildServiceProvider(true);
        ITerminal socket = provider.GetRequiredService<ITerminal>();
        TerminalOptions options = provider.GetRequiredService<IOptions<TerminalOptions>>().Value;
        bool registered = socket is not null && options.Endpoint == endpoint;
        Assert.True(registered, "ServiceCollectionExtensions does not register router socket and options");
    }

    /// <summary>
    /// Confirms that ConnectRouterHostedService opens router connection on start. Usage example: await service.StartAsync(token).
    /// </summary>
    [Fact(DisplayName = "ConnectRouterHostedService opens connection on startup")]
    public async Task Given_valid_options_when_host_starts_then_connects()
    {
        int port = RandomNumberGenerator.GetInt32(11_000, 58_000);
        Uri endpoint = new($"ws://127.0.0.1:{port}/router/");
        await using RouterSocketProbe socket = new();
        IOptions<TerminalOptions> options = Options.Create(new TerminalOptions { Endpoint = endpoint.ToString() });
        AlfaProTerminal service = new(socket, options);
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(2));
        await service.StartAsync(source.Token);
        bool connected = socket.Endpoint == endpoint;
        await service.StopAsync(source.Token);
        Assert.True(connected, "ConnectRouterHostedService does not open router connection");
    }

    /// <summary>
    /// Ensures that invalid endpoints produce failures on startup. Usage example: await service.StartAsync(token).
    /// </summary>
    [Fact(DisplayName = "ConnectRouterHostedService rejects invalid endpoint")]
    public async Task Given_invalid_endpoint_when_host_starts_then_throws()
    {
        await using RouterSocketProbe socket = new();
        IOptions<TerminalOptions> options = Options.Create(new TerminalOptions { Endpoint = "not-a-uri" });
        AlfaProTerminal service = new(socket, options);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.StartAsync(CancellationToken.None));
    }
}
