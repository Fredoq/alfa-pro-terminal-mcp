namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

/// <summary>
/// Configuration options for router connectivity. Usage example: bound from configuration section "Router"; override via env variable ROUTER__ENDPOINT.
/// </summary>
internal sealed class RouterOptions
{
    public string Endpoint { get; init; } = "ws://127.0.0.1:3366/router/";
}
