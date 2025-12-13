namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Globalization;
using System.Security.Cryptography;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Verifies CfgTerminalTimeout resolves timeout duration from configuration. Usage example: TimeSpan duration = new CfgTerminalTimeout(section).Duration().
/// </summary>
public sealed class CfgTerminalTimeoutTests
{
    /// <summary>
    /// Ensures that CfgTerminalTimeout uses configuration value when present. Usage example: TimeSpan duration = timeout.Duration().
    /// </summary>
    [Fact(DisplayName = "CfgTerminalTimeout uses configured timeout")]
    public void Given_configured_timeout_when_duration_then_returns_value()
    {
        int millis = RandomNumberGenerator.GetInt32(1, 60_000);
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Timeout"] = millis.ToString(CultureInfo.InvariantCulture) }).Build();
        CfgTerminalTimeout timeout = new(config);
        TimeSpan duration = timeout.Duration();
        TimeSpan expected = TimeSpan.FromMilliseconds(millis);
        Assert.True(duration == expected, "CfgTerminalTimeout does not return configured duration");
    }

    /// <summary>
    /// Confirms that CfgTerminalTimeout falls back to default when configuration value is missing. Usage example: TimeSpan duration = timeout.Duration().
    /// </summary>
    [Fact(DisplayName = "CfgTerminalTimeout uses default timeout when missing")]
    public void Given_missing_timeout_when_duration_then_returns_default()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();
        CfgTerminalTimeout timeout = new(config);
        TimeSpan duration = timeout.Duration();
        TimeSpan expected = TimeSpan.FromMilliseconds(5000);
        Assert.True(duration == expected, "CfgTerminalTimeout does not return default duration");
    }

    /// <summary>
    /// Verifies that CfgTerminalTimeout fails fast for invalid timeout values. Usage example: timeout.Duration().
    /// </summary>
    [Fact(DisplayName = "CfgTerminalTimeout throws for invalid timeout")]
    public void Given_invalid_timeout_when_duration_then_throws()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Timeout"] = $"пять-{Guid.NewGuid()}" }).Build();
        CfgTerminalTimeout timeout = new(config);
        Assert.Throws<InvalidOperationException>(() => timeout.Duration());
    }
}
