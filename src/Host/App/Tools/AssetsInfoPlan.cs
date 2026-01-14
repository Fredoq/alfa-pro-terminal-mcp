using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for asset info by identifiers. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class AssetsInfoPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idObjects":{"type":"array","description":"Collection of IdObject values to extract","items":{"type":"integer"}}},"required":["idObjects"]}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"assets":{"type":"array","description":"Asset info entries for requested identifiers","items":{"type":"object","properties":{"IdObject":{"type":"integer","description":"Asset identifier"},"Ticker":{"type":"string","description":"Exchange ticker"},"ISIN":{"type":"string","description":"International security identifier"},"Name":{"type":"string","description":"Asset name"},"Description":{"type":"string","description":"Asset description"},"Nominal":{"type":"number","description":"Nominal value"},"IdObjectType":{"type":"integer","description":"Asset type identifier"},"IdObjectGroup":{"type":"integer","description":"Asset group identifier"},"IdObjectBase":{"type":"integer","description":"Base asset identifier"},"IdObjectFaceUnit":{"type":"integer","description":"Face value currency identifier"},"MatDateObject":{"type":"string","description":"Expiration date of asset"},"Instruments":{"type":"array","description":"Trading instrument details","items":{"type":"object","properties":{"IdFi":{"type":"integer","description":"Financial instrument identifier"},"RCode":{"type":"string","description":"Portfolio code"},"IsLiquid":{"type":"boolean","description":"Liquidity flag"},"IdMarketBoard":{"type":"integer","description":"Market identifier"}},"required":["IdFi","RCode","IsLiquid","IdMarketBoard"],"additionalProperties":false}}},"required":["IdObject","Ticker","ISIN","Name","Description","Nominal","IdObjectType","IdObjectGroup","IdObjectBase","IdObjectFaceUnit","MatDateObject","Instruments"],"additionalProperties":false}}},"required":["assets"],"additionalProperties":false}""");
        return new Tool { Name = "info", Title = "Asset info by identifiers", Description = "Returns asset info list for the given object identifiers.", InputSchema = input, OutputSchema = output, Annotations = new ToolAnnotations { ReadOnlyHint = true, IdempotentHint = true, OpenWorldHint = false, DestructiveHint = false } };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"idObjects":{"type":"array","description":"Collection of IdObject values to extract","items":{"type":"integer"}}},"required":["idObjects"]}"""));
        return new MappedPayload(data, schema);
    }
}
