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
    /// Returns a tool result with structured content wrapped in the provided root property. Usage example: CallToolResult result = formatter.Result(entries, "accounts").
    /// </summary>
    /// <param name="entries">Structured entries payload.</param>
    /// <param name="name">Root property name.</param>
    public CallToolResult Result(IEntries entries, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidOperationException("Root name is missing");
        }
        string json = entries.Json() ?? throw new InvalidOperationException("Payload is missing");
        JsonNode node = JsonNode.Parse(json) ?? throw new InvalidOperationException("Payload is missing");
        JsonObject root = new() { [name] = node };
        string text = root.ToJsonString();
        return new CallToolResult { StructuredContent = root, Content = [new TextContentBlock { Text = text }] };
    }
}
