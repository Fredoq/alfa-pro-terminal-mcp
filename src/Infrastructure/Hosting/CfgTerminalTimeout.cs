using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

/// <summary>
/// Terminal timeout.
/// Usage example: ITerminalTimeout timeout = new CfgTerminalTimeout(section); TimeSpan duration = timeout.Duration();
/// </summary>
internal sealed record CfgTerminalTimeout : ITerminalTimeout
{
    private readonly string _timeout;

    /// <summary>
    /// Creates timeout backed by configuration section "Terminal".
    /// Usage example: new CfgTerminalTimeout(configuration.GetSection("Terminal")).
    /// </summary>
    /// <param name="configuration">Configuration section</param>
    public CfgTerminalTimeout(IConfiguration configuration) : this(configuration["Timeout"] is { Length: > 0 } value ? value : "5000")
    {
    }

    /// <summary>
    /// Creates timeout backed by a textual millisecond value.
    /// Usage example: new CfgTerminalTimeout("5000").
    /// </summary>
    /// <param name="timeout">Timeout in milliseconds</param>
    public CfgTerminalTimeout(string timeout)
    {
        _timeout = timeout;
    }

    /// <summary>
    /// Returns the timeout duration.
    /// Usage example: TimeSpan duration = timeout.Duration().
    /// </summary>
    public TimeSpan Duration()
    {
        if (!int.TryParse(_timeout, out int value) || value <= 0)
        {
            throw new InvalidOperationException("Terminal timeout is invalid");
        }
        return TimeSpan.FromMilliseconds(value);
    }
}
