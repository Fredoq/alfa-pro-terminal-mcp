using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;

/// <summary>
/// Defines configuration builder part. Usage example: IConfigurationBuilder builder = part.AddPart().
/// </summary>
internal interface IConfigPart
{
    /// <summary>
    /// Adds part to configuration builder and returns it. Usage example: IConfigurationBuilder builder = part.AddPart().
    /// </summary>
    IConfigurationBuilder AddPart();
}
