namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides base path value. Usage example: IBasePath path = new AppBasePath().
/// </summary>
internal sealed class AppBasePath : IBasePath
{
    private readonly string _path;

    /// <summary>
    /// Creates base path wrapper. Usage example: IBasePath path = new AppBasePath().
    /// </summary>
    public AppBasePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _path = path;
    }

    /// <summary>
    /// Creates base path wrapper with default value. Usage example: IBasePath path = new AppBasePath().
    /// </summary>
    public AppBasePath() : this(AppContext.BaseDirectory)
    {
    }

    /// <summary>
    /// Returns base path. Usage example: string path = item.Path().
    /// </summary>
    public string Path() => _path;
}
