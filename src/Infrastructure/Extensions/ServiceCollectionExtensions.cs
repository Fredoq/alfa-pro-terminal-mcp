using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Configuration;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Extensions;
/// <summary>
/// Registers infrastructure services in the dependency injection container. Usage example: services.Register();
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the router socket as a singleton service. Usage example: services.Register().
    /// </summary>
    public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        return services
            .AddOptions<RouterOptions>()
            .Bind(configuration.GetSection("Router"))
            .Validate(options => Uri.TryCreate(options.Endpoint, UriKind.Absolute, out _), "Router endpoint is invalid")
            .ValidateOnStart()
            .Services
            .AddSingleton<IRouterSocket, RouterSocket>()
            .AddHostedService<ConnectRouterHostedService>();
    }
}
