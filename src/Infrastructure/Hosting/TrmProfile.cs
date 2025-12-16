using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

/// <summary>
/// Provides terminal endpoint and timeout values backed by configuration.
/// Usage example: ITerminalProfile profile = new TrmProfile(configuration.GetSection("Terminal")); Uri uri = profile.Address(); TimeSpan duration = profile.Duration().
/// </summary>
public sealed record TrmProfile : ITerminalProfile
{
    private readonly string _endpoint;
    private readonly string _timeout;

    /// <summary>
    /// Creates profile backed by a configuration section.
    /// Usage example: new TrmProfile(configuration.GetSection("Terminal")).
    /// </summary>
    /// <param name="config">Configuration section</param>
    public TrmProfile(IConfiguration config)
        : this((config ?? throw new ArgumentNullException(nameof(config)))["Endpoint"] is { Length: > 0 } endpoint ? endpoint : "ws://127.0.0.1:3366/router/",
                config["Timeout"] is { Length: > 0 } timeout ? timeout : "5000")
    {
    }

    /// <summary>
    /// Creates profile backed by default values.
    /// Usage example: new TrmProfile().
    /// </summary>
    public TrmProfile() : this("ws://127.0.0.1:3366/router/", "5000")
    {
    }

    /// <summary>
    /// Creates profile backed by textual endpoint and millisecond timeout values.
    /// Usage example: new TrmProfile("ws://127.0.0.1:3366/router/", "5000").
    /// </summary>
    /// <param name="endpoint">Absolute router WebSocket endpoint</param>
    /// <param name="timeout">Timeout in milliseconds</param>
    public TrmProfile(string endpoint, string timeout)
    {
        _endpoint = endpoint;
        _timeout = timeout;
    }

    /// <inheritdoc />
    public Uri Address()
    {
        if (!Uri.TryCreate(_endpoint, UriKind.Absolute, out Uri? uri))
        {
            throw new InvalidOperationException("Terminal endpoint is invalid");
        }
        return uri;
    }

    /// <inheritdoc />
    public TimeSpan Duration()
    {
        if (!int.TryParse(_timeout, out int amount) || amount <= 0)
        {
            throw new InvalidOperationException("Terminal timeout is invalid");
        }
        return TimeSpan.FromMilliseconds(amount);
    }
}
