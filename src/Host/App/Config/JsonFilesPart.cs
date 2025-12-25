using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Adds json configuration files to the builder. Usage example: IConfigPart part = new JsonFilesPart(basePart, new[] { "appsettings.json" }).
/// </summary>
internal sealed class JsonFilesPart : IConfigPart
{
    private readonly IConfigPart _part;
    private readonly IReadOnlyCollection<string> _names;

    /// <summary>
    /// Creates json files part. Usage example: IConfigPart part = new JsonFilesPart(basePart, new[] { "appsettings.json" }).
    /// </summary>
    /// <param name="part">Inner configuration part.</param>
    /// <param name="names">Json file names.</param>
    public JsonFilesPart(IConfigPart part, IReadOnlyCollection<string> names)
    {
        _part = part;
        _names = names;
    }

    /// <summary>
    /// Creates json files part with default files. Usage example: IConfigPart part = new JsonFilesPart(basePart).
    /// </summary>
    /// <param name="part">Inner configuration part.</param>
    public JsonFilesPart(IConfigPart part)
        : this(part, ["appsettings.json", $"appsettings.{new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production").Name()}.json"])
    {
    }

    /// <summary>
    /// Adds json files to builder and returns it. Usage example: IConfigurationBuilder builder = part.AddPart().
    /// </summary>
    public IConfigurationBuilder AddPart()
    {
        IConfigurationBuilder builder = _part.AddPart();
        foreach (string name in _names)
        {
            if (name.Length == 0)
            {
                throw new InvalidOperationException("Configuration file name is missing");
            }
            builder.AddJsonFile(name, optional: true, reloadOnChange: false);
        }
        return builder;
    }
}
