namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

/// <summary>
/// Ensures router connection is established at startup. Usage example: registered as hosted service.
/// </summary>
internal sealed class ConnectRouterHostedService : IHostedService
{
    private readonly IRouterSocket _socket;
    private readonly IOptions<RouterOptions> _options;

    /// <summary>
    /// Creates the hosted service that connects the router. Usage example: new ConnectRouterHostedService(socket, endpoint).
    /// </summary>
    public ConnectRouterHostedService(IRouterSocket socket, IOptions<RouterOptions> options)
    {
        ArgumentNullException.ThrowIfNull(socket);
        ArgumentNullException.ThrowIfNull(options);
        _socket = socket;
        _options = options;
    }

    /// <summary>
    /// Connects to the router when the host starts. Usage example: called by infrastructure.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(_options.Value.Endpoint, UriKind.Absolute, out Uri? parsed))
        {
            throw new InvalidOperationException("Router endpoint is invalid");
        }
        return _socket.Connect(parsed, cancellationToken);
    }

    /// <summary>
    /// Closes router connection on shutdown. Usage example: called by infrastructure.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => _socket.Close(cancellationToken);
}
