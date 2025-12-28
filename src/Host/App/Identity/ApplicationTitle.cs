using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides application title value. Usage example: IApplicationTitle title = new ApplicationTitle("Alfa Pro Terminal MCP").
/// </summary>
internal sealed class ApplicationTitle : IApplicationTitle
{
    private readonly string _title;

    /// <summary>
    /// Creates application title wrapper. Usage example: IApplicationTitle title = new ApplicationTitle("Alfa Pro Terminal MCP").
    /// </summary>
    /// <param name="title">Title value.</param>
    public ApplicationTitle(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title);
        _title = title;
    }

    /// <summary>
    /// Returns application title. Usage example: string title = item.Title().
    /// </summary>
    public string Title() => _title;
}
