using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Builds configuration root from files and environment. Usage example: IConfig config = new Config(path, name).
/// </summary>
internal sealed class Config : IConfig
{
    private readonly IBasePath _path;
    private readonly IEnvironmentName _name;

    /// <summary>
    /// Creates configuration builder wrapper. Usage example: IConfig config = new Config(path, name).
    /// </summary>
    /// <param name="path">Base path provider.</param>
    /// <param name="name">Environment name provider.</param>
    public Config(IBasePath path, IEnvironmentName name)
    {
        _path = path;
        _name = name;
    }

    /// <summary>
    /// Returns configuration root. Usage example: IConfigurationRoot root = config.Root().
    /// </summary>
    public IConfigurationRoot Root()
    {
        ConfigurationBuilder builder = new();
        builder.SetBasePath(_path.Path());
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        builder.AddJsonFile($"appsettings.{_name.Name()}.json", optional: true, reloadOnChange: false);
        builder.AddEnvironmentVariables();
        IConfigurationRoot root = builder.Build();
        return root;
    }
}
