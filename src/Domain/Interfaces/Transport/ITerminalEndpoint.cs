namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;

/// <summary>
/// Provides an endpoint address for connecting to the terminal router.
/// Usage example: Uri uri = endpoint.Address();
/// </summary>
public interface ITerminalEndpoint
{
    /// <summary>
    /// Returns the endpoint address.
    /// Usage example: Uri uri = endpoint.Address();
    /// </summary>
    Uri Address();
}
