using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Config;

/// <summary>
/// Builds configuration root from parts. Usage example: IConfig config = new Config(part).
/// </summary>
internal sealed class Config : IConfig
{
    private readonly IConfigPart _part;

    /// <summary>
    /// Creates configuration root wrapper. Usage example: IConfig config = new Config(part).
    /// </summary>
    /// <param name="part">Configuration builder part.</param>
    public Config(IConfigPart part)
    {
        _part = part;
    }

    /// <summary>
    /// Returns configuration root. Usage example: IConfigurationRoot root = config.Root().
    /// </summary>
    public IConfigurationRoot Root()
        => _part.AddPart().Build();
}
