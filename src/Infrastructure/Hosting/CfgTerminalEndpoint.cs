using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

/// <summary>
/// Terminal endpoint
/// </summary>
internal record CfgTerminalEndpoint : ITerminalEndpoint
{
    private readonly string _endpoint;

    public CfgTerminalEndpoint(IConfiguration configuration) : this(configuration["Endpoint"] is { Length: > 0 } value ? value : "ws://127.0.0.1:3366/router/")
    {
    }

    public CfgTerminalEndpoint() : this("ws://127.0.0.1:3366/router/")
    { }

    public CfgTerminalEndpoint(string endpoint)
    {
        _endpoint = endpoint;
    }



    public Uri Address()
    {
        if (!Uri.TryCreate(_endpoint, UriKind.Absolute, out Uri? uri))
        {
            throw new InvalidOperationException("Terminal endpoint is invalid");
        }
        return uri;
    }
}
