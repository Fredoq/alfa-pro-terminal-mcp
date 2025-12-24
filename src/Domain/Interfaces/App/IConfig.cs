using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines configuration root retrieval. Usage example: IConfigurationRoot root = config.Root().
/// </summary>
public interface IConfig
{
    /// <summary>
    /// Returns configuration root. Usage example: IConfigurationRoot root = config.Root().
    /// </summary>
    IConfigurationRoot Root();
}
