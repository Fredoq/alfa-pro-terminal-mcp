using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Archive;

/// <summary>
/// Archive history retrieval contract.
/// </summary>
public interface IArchive
{
    /// <summary>
    /// Returns archive candles for the specified parameters. Usage example: var entries = await archive.History(1, 0, "hour", 1, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow, token);.
    /// </summary>
    /// <param name="idFi">Financial instrument identifier.</param>
    /// <param name="candleType">Candle kind: 0 for OHLCV, 2 for MPV.</param>
    /// <param name="interval">Timeframe unit: second, minute, hour, day, week, month.</param>
    /// <param name="period">Interval multiplier.</param>
    /// <param name="firstDay">First requested day (inclusive).</param>
    /// <param name="lastDay">Last requested day (inclusive).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Archive entries builder.</returns>
    Task<IEntries> History(long idFi, int candleType, string interval, int period, DateTime firstDay, DateTime lastDay, CancellationToken cancellationToken = default);
}
