using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Represents an empty entries array. Usage example: JsonNode node = new EmptyEntries().StructuredContent().
/// </summary>
internal sealed class EmptyEntries : IEntries
{
    /// <summary>
    /// Creates empty entries. Usage example: var entries = new EmptyEntries().
    /// </summary>
    public EmptyEntries()
    {
    }

    /// <summary>
    /// Returns an empty array as structured content. Usage example: JsonNode node = entries.StructuredContent().
    /// </summary>
    public JsonNode StructuredContent() => new JsonArray();
}
