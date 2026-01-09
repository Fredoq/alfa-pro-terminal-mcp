using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a string value from a JSON node by property name and maps JSON null to empty string. Usage example: string ticker = new JsonString(node, "Ticker").Value().
/// </summary>
public sealed record JsonString(JsonObject Node, string Name) : IJsonValue<string>
{
    /// <summary>
    /// Returns the string value extracted from the JSON node. Usage example: string name = new JsonString(node, "Name").Value().
    /// </summary>
    public string Value()
    {
        ArgumentException.ThrowIfNullOrEmpty(Name);
        if (!Node.TryGetPropertyValue(Name, out JsonNode? value))
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        if (value is null)
        {
            return string.Empty;
        }
        try
        {
            return value.GetValue<string>() ?? string.Empty;
        }
        catch (Exception)
        {
            if (value.ToJsonString() == "null")
            {
                return string.Empty;
            }
            throw new InvalidOperationException($"{Name} is missing");
        }
    }
}
