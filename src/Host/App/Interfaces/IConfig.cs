using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines configuration root retrieval. Usage example: IConfigurationRoot root = config.Root().
/// </summary>
internal interface IConfig
{
    /// <summary>
    /// Returns configuration root. Usage example: IConfigurationRoot root = config.Root().
    /// </summary>
    IConfigurationRoot Root();
}
