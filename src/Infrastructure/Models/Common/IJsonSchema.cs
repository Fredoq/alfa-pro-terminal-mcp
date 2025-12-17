using System.Text.Json;
using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Defines a JSON output schema for a single JSON element. Usage example: JsonNode node = schema.Node(element).
/// </summary>
internal interface IJsonSchema
{
    /// <summary>
    /// Returns the schema output node for the provided JSON element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    /// <param name="node">Source JSON element.</param>
    JsonNode Node(JsonElement node);
}

