using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Defines an output schema for a single OHLCV candle entry. Usage example: JsonNode node = new OhlcvSchema().Node(element).
/// </summary>
internal sealed class OhlcvSchema : IJsonSchema
{
    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node)
    {
        ArchiveDescriptions text = new();
        RulesSchema schema = new([
            new ValueRule<double>(new JsonDouble(node, "Open"), "Open", text.Text("Open")),
            new ValueRule<double>(new JsonDouble(node, "Close"), "Close", text.Text("Close")),
            new ValueRule<double>(new JsonDouble(node, "Low"), "Low", text.Text("Low")),
            new ValueRule<double>(new JsonDouble(node, "High"), "High", text.Text("High")),
            new ValueRule<long>(new JsonInteger(node, "Volume"), "Volume", text.Text("Volume")),
            new ValueRule<long>(new JsonInteger(node, "VolumeAsk"), "VolumeAsk", text.Text("VolumeAsk")),
            new ValueRule<long>(new JsonInteger(node, "OpenInt"), "OpenInt", text.Text("OpenInt")),
            new ValueRule<string>(new JsonString(node, "DT"), "Time", text.Text("Time"))
        ]);
        return schema.Node(node);
    }
}
