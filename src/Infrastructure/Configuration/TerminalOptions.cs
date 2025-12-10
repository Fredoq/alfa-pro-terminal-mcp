namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Configuration;

/// <summary>
/// Configuration options for terminal connectivity. Usage example: bound from configuration section "Terminal"; override via env variable TERMINAL__ENDPOINT.
/// </summary>
internal sealed class TerminalOptions
{
    public string Endpoint { get; init; } = "ws://127.0.0.1:3366/router/";
    public int Timeout { get; init; } = 5000;
}
