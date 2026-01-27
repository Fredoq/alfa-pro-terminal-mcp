using System.Threading;
using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Trading;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;

/// <summary>
/// Provides instrument details for trading. Usage example: InstrumentValue item = await instrument.Value(asset, token).
/// </summary>
public interface IInstrument
{
    /// <summary>
    /// Returns instrument details for the specified asset identifier. Usage example: InstrumentValue item = await instrument.Value(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Instrument details.</returns>
    Task<InstrumentValue> Value(long asset, CancellationToken token = default);

    /// <summary>
    /// Returns object group identifier for the specified asset. Usage example: long id = await instrument.Group(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Object group identifier.</returns>
    Task<long> Group(long asset, CancellationToken token = default);

    /// <summary>
    /// Returns market board identifier for the specified asset. Usage example: long id = await instrument.Market(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Market board identifier.</returns>
    Task<long> Market(long asset, CancellationToken token = default);

    /// <summary>
    /// Returns portfolio code for the specified asset. Usage example: string code = await instrument.Code(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Portfolio code.</returns>
    Task<string> Code(long asset, CancellationToken token = default);
}
