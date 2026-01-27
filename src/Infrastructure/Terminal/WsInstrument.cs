using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Trading;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Resolves instrument identifiers and portfolio code for assets. Usage example: InstrumentValue item = await new WsInstrument(terminal, log).Value(asset, token).
/// </summary>
public sealed class WsInstrument : IInstrument
{
    private readonly WsAssetsInfo source;
    private InstrumentValue item;
    private long asset;
    private bool flag;

    /// <summary>
    /// Creates an instrument resolver bound to the terminal. Usage example: var resolver = new WsInstrument(terminal, log).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="log">Logger instance.</param>
    public WsInstrument(ITerminal terminal, ILogger log)
    {
        source = new WsAssetsInfo(terminal, log);
        item = new InstrumentValue(0, 0, string.Empty);
        asset = 0;
        flag = false;
    }

    /// <summary>
    /// Returns instrument details for the specified asset identifier. Usage example: InstrumentValue item = await resolver.Value(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Instrument details.</returns>
    public async Task<InstrumentValue> Value(long asset, CancellationToken token = default)
    {
        if (flag && asset == this.asset)
        {
            return item;
        }
        string text = JsonSerializer.Serialize(new { IdObjects = new long[] { asset } });
        IEntries entries = await source.Entries(new TextPayload(text), token);
        JsonObject root = entries.StructuredContent().AsObject();
        if (!root.TryGetPropertyValue("assets", out JsonNode? data) || data is null)
        {
            throw new InvalidOperationException("Asset infos are missing");
        }
        JsonArray list = data.AsArray();
        JsonObject info = new();
        bool mark = false;
        foreach (JsonNode? entry in list)
        {
            if (entry is null)
            {
                throw new InvalidOperationException("Entry node is missing");
            }
            JsonObject value = entry.AsObject();
            if (mark)
            {
                throw new InvalidOperationException("Multiple asset infos are matched");
            }
            info = value;
            mark = true;
        }
        if (!mark)
        {
            throw new InvalidOperationException("Asset info is missing");
        }
        if (!info.TryGetPropertyValue("Instruments", out JsonNode? items) || items is null)
        {
            throw new InvalidOperationException("Instruments are missing");
        }
        JsonArray instruments = items.AsArray();
        JsonObject node = new();
        bool picked = false;
        foreach (JsonNode? entry in instruments)
        {
            if (entry is null)
            {
                throw new InvalidOperationException("Instrument node is missing");
            }
            JsonObject value = entry.AsObject();
            if (!picked)
            {
                node = value;
                picked = true;
            }
            if (new JsonBool(value, "IsLiquid").Value())
            {
                node = value;
                picked = true;
                break;
            }
        }
        if (!picked)
        {
            throw new InvalidOperationException("Instrument is missing");
        }
        long group = new JsonInteger(info, "IdObjectGroup").Value();
        long market = new JsonInteger(node, "IdMarketBoard").Value();
        string code = new JsonString(node, "RCode").Value();
        item = new InstrumentValue(group, market, code);
        this.asset = asset;
        flag = true;
        return item;
    }

    /// <summary>
    /// Returns object group identifier for the specified asset. Usage example: long id = await resolver.Group(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Object group identifier.</returns>
    public async Task<long> Group(long asset, CancellationToken token = default) => (await Value(asset, token)).Group;

    /// <summary>
    /// Returns market board identifier for the specified asset. Usage example: long id = await resolver.Market(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Market board identifier.</returns>
    public async Task<long> Market(long asset, CancellationToken token = default) => (await Value(asset, token)).Market;

    /// <summary>
    /// Returns portfolio code for the specified asset. Usage example: string code = await resolver.Code(asset, token).
    /// </summary>
    /// <param name="asset">Asset identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Portfolio code.</returns>
    public async Task<string> Code(long asset, CancellationToken token = default) => (await Value(asset, token)).Code;
}
