using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides process path value. Usage example: IProcessPath path = new ProcessPath().
/// </summary>
internal sealed class ProcessPath : IProcessPath
{
    /// <summary>
    /// Creates process path wrapper. Usage example: IProcessPath path = new ProcessPath().
    /// </summary>
    public ProcessPath()
    {
    }

    /// <summary>
    /// Returns process path. Usage example: string path = item.Path().
    /// </summary>
    public string Path()
    {
        string path = Environment.ProcessPath ?? string.Empty;
        if (path.Length > 0)
        {
            return path;
        }
        path = AppContext.BaseDirectory;
        if (path.Length == 0)
        {
            throw new InvalidOperationException("Process path is missing");
        }
        return path;
    }
}
