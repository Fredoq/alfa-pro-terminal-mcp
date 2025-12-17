using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Defines an output schema for a single MPV candle entry. Usage example: JsonNode node = new MpvSchema().Node(element).
/// </summary>
internal sealed class MpvSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an MPV schema with described fields and levels. Usage example: var schema = new MpvSchema().
    /// </summary>
    public MpvSchema()
    {
        ArchiveDescriptions text = new();
        JsonDoubleItem doubley = new();
        JsonStringItem stringy = new();
        _schema = new RulesSchema([
            new ValueRule<double>(doubley, "Open", text.Text("Open")),
            new ValueRule<double>(doubley, "Close", text.Text("Close")),
            new ValueRule<string>(stringy, "Time", "DT", text.Text("Time")),
            new LevelsRule()
        ]);
    }

    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
