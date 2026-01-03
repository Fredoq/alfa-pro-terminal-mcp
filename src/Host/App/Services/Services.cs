using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Microsoft.Extensions.DependencyInjection;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Services;

/// <summary>
/// Wraps service provider lifetime. Usage example: IServices services = new Services(provider).
/// </summary>
internal sealed class Services : IServices
{
    private readonly ServiceProvider _provider;

    /// <summary>
    /// Creates provider wrapper. Usage example: IServices services = new Services(provider).
    /// </summary>
    /// <param name="provider">Service provider instance.</param>
    public Services(ServiceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Returns service provider. Usage example: ServiceProvider provider = item.Provider().
    /// </summary>
    public ServiceProvider Provider() => _provider;

    /// <summary>
    /// Releases the provider. Usage example: await item.Release().
    /// </summary>
    public ValueTask Release() => _provider.DisposeAsync();
}
