using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

/// <summary>
/// Defines a JSON output schema for a single JSON object. Usage example: JsonNode node = schema.Node(item).
/// </summary>
internal interface IJsonSchema
{
    /// <summary>
    /// Returns the schema output node for the provided JSON object. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    JsonNode Node(JsonObject node);
}
