using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Defines an output schema for a single OHLCV candle entry. Usage example: JsonNode node = new OhlcvSchema().Node(element).
/// </summary>
internal sealed class OhlcvSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an OHLCV schema with described fields. Usage example: var schema = new OhlcvSchema().
    /// </summary>
    public OhlcvSchema()
    {
        ArchiveDescriptions text = new();
        JsonIntegerItem integer = new();
        JsonDoubleItem doubley = new();
        JsonStringItem stringy = new();
        _schema = new RulesSchema([
            new ValueRule<double>(doubley, "Open", text.Text("Open")),
            new ValueRule<double>(doubley, "Close", text.Text("Close")),
            new ValueRule<double>(doubley, "Low", text.Text("Low")),
            new ValueRule<double>(doubley, "High", text.Text("High")),
            new ValueRule<long>(integer, "Volume", text.Text("Volume")),
            new ValueRule<long>(integer, "VolumeAsk", text.Text("VolumeAsk")),
            new ValueRule<long>(integer, "OpenInt", text.Text("OpenInt")),
            new ValueRule<string>(stringy, "Time", "DT", text.Text("Time"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
