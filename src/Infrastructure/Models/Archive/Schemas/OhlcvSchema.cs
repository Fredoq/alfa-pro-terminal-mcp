using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Descriptions;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;

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
        _schema = new RulesSchema([
            new RealRule("Open", text.Text("Open")),
            new RealRule("Close", text.Text("Close")),
            new RealRule("Low", text.Text("Low")),
            new RealRule("High", text.Text("High")),
            new WholeRule("Volume", text.Text("Volume")),
            new WholeRule("VolumeAsk", text.Text("VolumeAsk")),
            new WholeRule("OpenInt", text.Text("OpenInt")),
            new TextRule("Time", "DT", text.Text("Time"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
