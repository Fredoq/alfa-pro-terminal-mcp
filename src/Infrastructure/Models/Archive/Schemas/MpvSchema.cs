using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Descriptions;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;

/// <summary>
/// Defines an output schema for a single MPV candle entry. Usage example: JsonNode node = new MpvSchema().Node(element).
/// </summary>
internal sealed class MpvSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an MPV schema with described fields. Usage example: var schema = new MpvSchema().
    /// </summary>
    public MpvSchema()
    {
        ArchiveDescriptions text = new();
        _schema = new RulesSchema([
            new RealRule("Open", text.Text("Open")),
            new RealRule("Close", text.Text("Close")),
            new TextRule("Time", "DT", text.Text("Time")),
            new LevelsRule()
        ]);
    }

    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
