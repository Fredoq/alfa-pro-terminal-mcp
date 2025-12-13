namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

/// <summary>
/// Provides a timeout duration used by terminal infrastructure.
/// Usage example: TimeSpan duration = timeout.Duration();
/// </summary>
public interface ITerminalTimeout
{
    /// <summary>
    /// Returns a timeout duration.
    /// Usage example: using var source = new CancellationTokenSource(timeout.Duration());
    /// </summary>
    TimeSpan Duration();
}
