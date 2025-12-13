namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Verifies CfgTerminalEndpoint resolves terminal endpoint address from configuration. Usage example: Uri uri = new CfgTerminalEndpoint(section).Address().
/// </summary>
public sealed class CfgTerminalEndpointTests
{
    /// <summary>
    /// Ensures that CfgTerminalEndpoint uses configuration value when present. Usage example: Uri uri = endpoint.Address().
    /// </summary>
    [Fact(DisplayName = "CfgTerminalEndpoint uses configured endpoint")]
    public void Given_configured_endpoint_when_address_then_returns_uri()
    {
        int port = RandomNumberGenerator.GetInt32(1025, 65000);
        string value = $"ws://127.0.0.1:{port}/маршрут/{Guid.NewGuid()}/";
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = value }).Build();
        CfgTerminalEndpoint endpoint = new(config);
        Uri address = endpoint.Address();
        Uri expected = new(value);
        Assert.True(address == expected, "CfgTerminalEndpoint does not return configured endpoint");
    }

    /// <summary>
    /// Confirms that CfgTerminalEndpoint falls back to default when configuration value is missing. Usage example: Uri uri = endpoint.Address().
    /// </summary>
    [Fact(DisplayName = "CfgTerminalEndpoint uses default endpoint when missing")]
    public void Given_missing_endpoint_when_address_then_returns_default()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();
        CfgTerminalEndpoint endpoint = new(config);
        Uri address = endpoint.Address();
        Uri expected = new("ws://127.0.0.1:3366/router/");
        Assert.True(address == expected, "CfgTerminalEndpoint does not return default endpoint");
    }

    /// <summary>
    /// Verifies that CfgTerminalEndpoint fails fast for invalid endpoint values. Usage example: endpoint.Address().
    /// </summary>
    [Fact(DisplayName = "CfgTerminalEndpoint throws for invalid endpoint")]
    public void Given_invalid_endpoint_when_address_then_throws()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = $"не-uri-{Guid.NewGuid()}" }).Build();
        CfgTerminalEndpoint endpoint = new(config);
        Assert.Throws<InvalidOperationException>(() => endpoint.Address());
    }
}

