using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a boolean value from a JSON node by property name. Usage example: bool liquid = new JsonBool(node, "IsLiquid").Value().
/// </summary>
public sealed record JsonBool(JsonObject Node, string Name) : IJsonValue<bool>
{
    /// <summary>
    /// Returns the boolean value extracted from the JSON node. Usage example: bool ok = new JsonBool(node, "Enabled").Value().
    /// </summary>
    public bool Value()
    {
        ArgumentException.ThrowIfNullOrEmpty(Name);
        if (!Node.TryGetPropertyValue(Name, out JsonNode? value) || value is null)
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        try
        {
            return value.GetValue<bool>();
        }
        catch (Exception)
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
    }
}
