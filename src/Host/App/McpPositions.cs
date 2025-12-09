using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

/// <summary>
/// MCP tool wrapper for account positions. Usage example: string json = await new McpPositions(socket, logger).Positions(123).
/// </summary>
[McpServerToolType]
internal sealed class McpPositions
{
    private readonly ITerminal _routerSocket;
    private readonly ILogger<McpPositions> _logger;

    /// <summary>
    /// Creates a MCP positions tool. Usage example: var tool = new McpPositions(socket, logger).
    /// </summary>
    public McpPositions(ITerminal routerSocket, ILogger<McpPositions> logger)
    {
        ArgumentNullException.ThrowIfNull(routerSocket);
        ArgumentNullException.ThrowIfNull(logger);
        _routerSocket = routerSocket;
        _logger = logger;
    }

    /// <summary>
    /// Returns positions for the specified account with field descriptions. Usage example: string json = await tool.Positions(123).
    /// </summary>
    [McpServerTool, Description("Returns positions for the given account id with field descriptions.")]
    public async Task<string> Positions(long accountId) => (await new WsPositions(_routerSocket, _logger).Entries(accountId)).Json();

    /// <summary>
    /// Returns asset infos for the provided identifiers with field descriptions. Usage example: string json = await tool.Info(new[] { 1L, 2L }).
    /// </summary>
    [McpServerTool, Description("Returns asset info list for the given object identifiers with field descriptions.")]
    public async Task<string> Info([Description("Collection of IdObject values to extract.")] long[] idObjects) => (await new WsAssetsInfo(_routerSocket, _logger).Info(idObjects)).Json();

    /// <summary>
    /// Returns asset infos for the provided tickers with field descriptions. Usage example: string json = await tool.InfoByTickers(new[] { "AFLT", "GAZP" }).
    /// </summary>
    [McpServerTool, Description("Returns asset info list for the given ticker symbols with field descriptions.")]
    public async Task<string> InfoByTickers([Description("Collection of ticker symbols to extract.")] string[] tickers) => (await new WsAssetsInfo(_routerSocket, _logger).InfoByTickers(tickers)).Json();

    /// <summary>
    /// Returns archive candles for the specified parameters with field descriptions. Usage example: string json = await tool.History(1, 0, "hour", 1, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow).
    /// </summary>
    [McpServerTool, Description("Returns archive candles for given instrument, candle type, interval, period, first day and last day with field descriptions. Prefer explicit timestamps for firstDay/lastDay, for example \"lastDay\": \"2025-12-08T23:59:00+03:00\".")]
    public async Task<string> History(
        [Description("Financial instrument identifier.")] long idFi,
        [Description("Candle kind: 0 for OHLCV, 2 for MPV.")] int candleType,
        [Description("Timeframe unit: second, minute, hour, day, week or month.")] string interval,
        [Description("Interval multiplier matching the interval unit.")] int period,
        [Description("First requested trading day inclusive.")] DateTime firstDay,
        [Description("Last requested trading day inclusive.")] DateTime lastDay)
        => (await new WsArchive(_routerSocket, _logger).History(idFi, candleType, interval, period, firstDay, lastDay)).Json();
}
