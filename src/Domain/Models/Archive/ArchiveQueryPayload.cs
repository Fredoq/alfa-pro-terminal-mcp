using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Archive;

/// <summary>
/// Represents payload for archive query routing. Usage example: var payload = new ArchiveQueryPayload(1, 0, "hour", 1, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow); string json = payload.AsString();.
/// </summary>
public sealed record ArchiveQueryPayload : IPayload
{
    private readonly long _idFi;
    private readonly int _candleType;
    private readonly string _interval;
    private readonly int _period;
    private readonly DateTime _firstDay;
    private readonly DateTime _lastDay;

    public ArchiveQueryPayload(long idFi, int candleType, string interval, int period, DateTime firstDay, DateTime lastDay)
    {
        ArgumentException.ThrowIfNullOrEmpty(interval);
        _idFi = idFi;
        _candleType = candleType;
        _interval = interval;
        _period = period;
        _firstDay = firstDay;
        _lastDay = lastDay;
    }

    /// <summary>
    /// Serializes payload into transport format. Usage example: string json = payload.AsString();.
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString() => System.Text.Json.JsonSerializer.Serialize(new
    {
        IdFi = _idFi,
        CandleType = _candleType,
        Interval = _interval,
        Period = _period,
        FirstDay = _firstDay,
        LastDay = _lastDay
    });
}
