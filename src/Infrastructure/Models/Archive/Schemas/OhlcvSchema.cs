using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive.Schemas;

/// <summary>
/// Defines an output schema for a single OHLCV candle entry. Usage example: JsonNode node = new OhlcvSchema().Node(item).
/// </summary>
internal sealed class OhlcvSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an OHLCV schema with fields. Usage example: var schema = new OhlcvSchema().
    /// </summary>
    public OhlcvSchema()
    {
        _schema = new RulesSchema([
            new RealRule("Open"),
            new RealRule("Close"),
            new RealRule("Low"),
            new RealRule("High"),
            new WholeRule("Volume"),
            new WholeRule("VolumeAsk"),
            new WholeRule("OpenInt"),
            new TextRule("Time", "DT")
        ]);
    }

    /// <summary>
    /// Returns an output node for the candle element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
