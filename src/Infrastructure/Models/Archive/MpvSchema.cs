using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Archive;

/// <summary>
/// Defines an output schema for a single MPV candle entry. Usage example: JsonNode node = new MpvSchema().Node(element).
/// </summary>
internal sealed class MpvSchema : IJsonSchema
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
            new ValueRule<string>(new JsonString(node, "DT"), "Time", text.Text("Time")),
            new LevelsRule()
        ]);
        return schema.Node(node);
    }
}
