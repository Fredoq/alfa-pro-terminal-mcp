using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides base path value. Usage example: IBasePath path = new BasePath().
/// </summary>
internal sealed class BasePath : IBasePath
{
    /// <summary>
    /// Creates base path wrapper. Usage example: IBasePath path = new BasePath().
    /// </summary>
    public BasePath()
    {
    }

    /// <summary>
    /// Returns base path. Usage example: string path = item.Path().
    /// </summary>
    public string Path()
    {
        string path = AppContext.BaseDirectory;
        if (path.Length == 0)
        {
            throw new InvalidOperationException("Base path is missing");
        }
        return path;
    }
}
