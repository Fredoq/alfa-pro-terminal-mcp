namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Extensions;

using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registers infrastructure services in the dependency injection container. Usage example: services.Register();
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the router socket as a singleton service. Usage example: services.Register().
    /// </summary>
    public static IServiceCollection Register(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        return services.AddSingleton<IRouterSocket, RouterSocket>();
    }
}
