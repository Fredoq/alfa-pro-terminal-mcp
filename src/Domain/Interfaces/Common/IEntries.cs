using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

/// <summary>
/// Represents entries payload with structured and text projections. Usage example: JsonNode node = entries.StructuredContent();.
/// </summary>
public interface IEntries
{
    /// <summary>
    /// Returns entries as structured JSON content. Usage example: JsonNode node = entries.StructuredContent();.
    /// </summary>
    JsonNode StructuredContent();

    /// <summary>
    /// Returns entries as JSON text. Usage example: string json = entries.Text();.
    /// </summary>
    string Text();
}
