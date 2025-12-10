using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Configuration;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
            .AddOptions<TerminalOptions>()
            .Bind(configuration.GetSection("Terminal"))
            .Validate(options => Uri.TryCreate(options.Endpoint, UriKind.Absolute, out _), "Terminal endpoint is invalid")
            .ValidateOnStart()
            .Services
            .AddSingleton(sp =>
            {
                IOptions<TerminalOptions> options = sp.GetRequiredService<IOptions<TerminalOptions>>();
                return new AlfaProTerminal(options);
            })
            .AddSingleton<ITerminal>(sp => sp.GetRequiredService<AlfaProTerminal>())
            .AddSingleton<IHostedService>(sp => sp.GetRequiredService<AlfaProTerminal>());
    }
}
