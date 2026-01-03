using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Config;

/// <summary>
/// Adds environment variables to the configuration builder. Usage example: IConfigPart part = new EnvironmentVariablesPart(basePart).
/// </summary>
internal sealed class EnvironmentVariablesPart : IConfigPart
{
    private readonly IConfigPart _part;

    /// <summary>
    /// Creates environment variables part. Usage example: IConfigPart part = new EnvironmentVariablesPart(basePart).
    /// </summary>
    /// <param name="part">Inner configuration part.</param>
    public EnvironmentVariablesPart(IConfigPart part)
    {
        _part = part;
    }

    /// <summary>
    /// Adds environment variables to builder and returns it. Usage example: IConfigurationBuilder builder = part.AddPart().
    /// </summary>
    public IConfigurationBuilder AddPart()
        => _part.AddPart().AddEnvironmentVariables();
}
