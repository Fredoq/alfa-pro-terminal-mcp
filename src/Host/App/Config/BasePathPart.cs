using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Adds base path to the configuration builder. Usage example: IConfigPart part = new BasePathPart(new ConfigurationBuilder(), path).
/// </summary>
internal sealed class BasePathPart : IConfigPart
{
    private readonly IConfigurationBuilder _builder;
    private readonly IBasePath _path;

    /// <summary>
    /// Creates base path part. Usage example: IConfigPart part = new BasePathPart(new ConfigurationBuilder(), path).
    /// </summary>
    /// <param name="builder">Configuration builder.</param>
    /// <param name="path">Base path provider.</param>
    public BasePathPart(IConfigurationBuilder builder, IBasePath path)
    {
        _builder = builder;
        _path = path;
    }

    /// <summary>
    /// Adds base path to builder and returns it. Usage example: IConfigurationBuilder builder = part.AddPart().
    /// </summary>
    public IConfigurationBuilder AddPart()
        => _builder.SetBasePath(_path.Path());
}
