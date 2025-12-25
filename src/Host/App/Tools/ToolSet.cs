using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Creates tool collection from terminal and formatter. Usage example: IToolSet tools = new ToolSet(terminal, logger, content).
/// </summary>
internal sealed class ToolSet : IToolSet
{
    private readonly ITerminal _terminal;
    private readonly ILogger _logger;
    private readonly IContent _content;

    /// <summary>
    /// Creates tool collection wrapper. Usage example: IToolSet tools = new ToolSet(terminal, logger, content).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="loggerFactory">Logger factory.</param>
    /// <param name="content">Response formatter.</param>
    public ToolSet(ITerminal terminal, ILoggerFactory loggerFactory, IContent content)
    {
        _terminal = terminal;
        _logger = loggerFactory.CreateLogger<ToolSet>();
        _content = content;
    }

    /// <summary>
    /// Returns tool collection. Usage example: IReadOnlyList&lt;IMcpTool&gt; list = item.Tools().
    /// </summary>
    public IReadOnlyList<IMcpTool> Tools()
    {
        IMcpTool[] list =
        [
            new AccountsEntriesTool(_terminal, _logger, _content),
            new AccountsBalanceTool(_terminal, _logger, _content),
            new PositionsTool(_terminal, _logger, _content),
            new AssetsInfoTool(_terminal, _logger, _content),
            new AssetsTickersTool(_terminal, _logger, _content),
            new ArchiveTool(_terminal, _logger, _content)
        ];
        return list;
    }
}
