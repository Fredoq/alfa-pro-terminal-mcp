using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Provides description texts for archive candle outputs. Usage example: string text = new ArchiveDescriptions().Text("Open").
/// </summary>
internal sealed class ArchiveDescriptions : IDescriptions
{
    private readonly Dictionary<string, string> _data = new()
    {
        ["Open"] = "Opening price",
        ["Close"] = "Closing price",
        ["Low"] = "Lowest price in timeframe",
        ["High"] = "Highest price in timeframe",
        ["Volume"] = "Traded volume in timeframe",
        ["VolumeAsk"] = "Ask volume in timeframe",
        ["OpenInt"] = "Open interest for futures",
        ["Time"] = "Candle timestamp",
        ["Levels"] = "Price levels for MPV candle",
        ["Price"] = "Price at level",
        ["QtyVolume"] = "Volume at level in timeframe",
        ["QtyVolumeAsk"] = "Ask volume at level in timeframe"
    };

    /// <summary>
    /// Returns a description text for the output field. Usage example: string text = descriptions.Text("Time").
    /// </summary>
    public string Text(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        if (_data.TryGetValue(name, out string? text))
        {
            return text;
        }
        throw new InvalidOperationException($"{name} description is missing");
    }
}
