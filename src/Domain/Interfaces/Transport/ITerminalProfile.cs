namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

/// <summary>
/// Provides endpoint and timeout settings for terminal infrastructure.
/// Usage example: Uri uri = profile.Address(); using var source = new CancellationTokenSource(profile.Duration()).
/// </summary>
public interface ITerminalProfile
{
    /// <summary>
    /// Returns the endpoint address.
    /// Usage example: Uri uri = profile.Address().
    /// </summary>
    Uri Address();

    /// <summary>
    /// Returns a timeout duration.
    /// Usage example: using var source = new CancellationTokenSource(profile.Duration()).
    /// </summary>
    TimeSpan Duration();
}
