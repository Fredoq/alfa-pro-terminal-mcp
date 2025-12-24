using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines service provider access. Usage example: ServiceProvider provider = item.Provider().
/// </summary>
public interface IServices
{
    /// <summary>
    /// Returns service provider. Usage example: ServiceProvider provider = item.Provider().
    /// </summary>
    ServiceProvider Provider();

    /// <summary>
    /// Releases the provider. Usage example: await item.Release().
    /// </summary>
    ValueTask Release();
}
