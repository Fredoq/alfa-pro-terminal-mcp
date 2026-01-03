using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Interfaces;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Content;

/// <summary>
/// Formats tool results with structured payloads and text blocks. Usage example: IContent formatter = new Content().
/// </summary>
internal sealed class Content : IContent
{
    /// <summary>
    /// Returns a tool result with structured content. Usage example: CallToolResult result = formatter.Result(entries).
    /// </summary>
    public CallToolResult Result(IEntries entries)
    {
        string json = entries.Json() ?? throw new InvalidOperationException("Payload is missing");
        JsonNode node = JsonNode.Parse(json) ?? throw new InvalidOperationException("Payload is missing");
        JsonObject data = new() { ["data"] = node };
        return new CallToolResult { StructuredContent = data, Content = [new TextContentBlock { Text = json }] };
    }
}
