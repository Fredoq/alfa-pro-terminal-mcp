namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Globalization;
using System.Security.Cryptography;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Verifies TrmProfile resolves terminal endpoint and timeout from configuration. Usage example: ITerminalProfile profile = new TrmProfile(section); Uri uri = profile.Address(); TimeSpan duration = profile.Duration().
/// </summary>
public sealed class TrmProfileTests
{
    /// <summary>
    /// Ensures that TrmProfile uses configured endpoint value when present. Usage example: Uri uri = profile.Address().
    /// </summary>
    [Fact(DisplayName = "TrmProfile uses configured endpoint")]
    public void Given_configured_endpoint_when_address_then_returns_uri()
    {
        int port = RandomNumberGenerator.GetInt32(1025, 65000);
        string endpoint = $"ws://127.0.0.1:{port}/маршрут/{Guid.NewGuid()}/";
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = endpoint, ["Timeout"] = "5000" }).Build();
        TrmProfile profile = new(config);
        Uri address = profile.Address();
        Uri expected = new(endpoint);
        Assert.True(address == expected, "TrmProfile does not return configured endpoint");
    }

    /// <summary>
    /// Confirms that TrmProfile falls back to default endpoint when configuration value is missing. Usage example: Uri uri = profile.Address().
    /// </summary>
    [Fact(DisplayName = "TrmProfile uses default endpoint when missing")]
    public void Given_missing_endpoint_when_address_then_returns_default()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Timeout"] = "5000" }).Build();
        TrmProfile profile = new(config);
        Uri address = profile.Address();
        Uri expected = new("ws://127.0.0.1:3366/router/");
        Assert.True(address == expected, "TrmProfile does not return default endpoint");
    }

    /// <summary>
    /// Verifies that TrmProfile fails fast for invalid endpoint values. Usage example: profile.Address().
    /// </summary>
    [Fact(DisplayName = "TrmProfile throws for invalid endpoint")]
    public void Given_invalid_endpoint_when_address_then_throws()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = $"не-uri-{Guid.NewGuid()}", ["Timeout"] = "5000" }).Build();
        TrmProfile profile = new(config);
        Assert.Throws<InvalidOperationException>(() => profile.Address());
    }

    /// <summary>
    /// Ensures that TrmProfile uses configured timeout value when present. Usage example: TimeSpan duration = profile.Duration().
    /// </summary>
    [Fact(DisplayName = "TrmProfile uses configured timeout")]
    public void Given_configured_timeout_when_duration_then_returns_value()
    {
        int millis = RandomNumberGenerator.GetInt32(1, 60_000);
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = "ws://127.0.0.1:3366/router/", ["Timeout"] = millis.ToString(CultureInfo.InvariantCulture) }).Build();
        TrmProfile profile = new(config);
        TimeSpan duration = profile.Duration();
        TimeSpan expected = TimeSpan.FromMilliseconds(millis);
        Assert.True(duration == expected, "TrmProfile does not return configured duration");
    }

    /// <summary>
    /// Confirms that TrmProfile falls back to default timeout when configuration value is missing. Usage example: TimeSpan duration = profile.Duration().
    /// </summary>
    [Fact(DisplayName = "TrmProfile uses default timeout when missing")]
    public void Given_missing_timeout_when_duration_then_returns_default()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = "ws://127.0.0.1:3366/router/" }).Build();
        TrmProfile profile = new(config);
        TimeSpan duration = profile.Duration();
        TimeSpan expected = TimeSpan.FromMilliseconds(5000);
        Assert.True(duration == expected, "TrmProfile does not return default duration");
    }

    /// <summary>
    /// Verifies that TrmProfile fails fast for invalid timeout values. Usage example: profile.Duration().
    /// </summary>
    [Fact(DisplayName = "TrmProfile throws for invalid timeout")]
    public void Given_invalid_timeout_when_duration_then_throws()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Endpoint"] = "ws://127.0.0.1:3366/router/", ["Timeout"] = $"пять-{Guid.NewGuid()}" }).Build();
        TrmProfile profile = new(config);
        Assert.Throws<InvalidOperationException>(() => profile.Duration());
    }
}
