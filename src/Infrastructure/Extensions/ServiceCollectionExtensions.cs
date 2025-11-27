namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Extensions;

using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
            .AddSingleton<JsonSerializerOptions>(_ => CreateJsonOptions())
            .AddSingleton<IRouterSocket, RouterSocket>()
            .AddSingleton<ITerminal, WsTerminal>()
            .AddHostedService<ConnectRouterHostedService>();
    }

    /// <summary>
    /// Creates serializer options with custom converters. Usage example: JsonSerializerOptions options = CreateJsonOptions();.
    /// </summary>
    private static JsonSerializerOptions CreateJsonOptions()
    {
        JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
        options.Converters.Add(new AccountsJsonConverter());
        return options;
    }
}
