using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a string value from a JSON node by property name and maps JSON null to empty string. Usage example: string ticker = new JsonString(node, "Ticker").Value().
/// </summary>
public sealed record JsonString(JsonElement Node, string Name) : IJsonValue<string>
{
    /// <summary>
    /// Returns the string value extracted from the JSON node. Usage example: string name = new JsonString(node, "Name").Value().
    /// </summary>
    public string Value()
    {
        ArgumentException.ThrowIfNullOrEmpty(Name);
        if (!Node.TryGetProperty(Name, out JsonElement value))
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        if (value.ValueKind == JsonValueKind.String)
        {
            return value.GetString() ?? string.Empty;
        }
        if (value.ValueKind == JsonValueKind.Null)
        {
            return string.Empty;
        }
        throw new InvalidOperationException($"{Name} is missing");
    }
}
