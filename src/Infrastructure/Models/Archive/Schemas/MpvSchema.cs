using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;

/// <summary>
/// Defines an output schema for a single MPV candle entry. Usage example: JsonNode node = new MpvSchema().Node(item).
/// </summary>
internal sealed class MpvSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an MPV schema with fields. Usage example: var schema = new MpvSchema().
    /// </summary>
    public MpvSchema()
    {
        _schema = new RulesSchema([
            new RealRule("Open"),
            new RealRule("Close"),
            new TextRule("Time", "DT"),
            new LevelsRule()
        ]);
    }

    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
