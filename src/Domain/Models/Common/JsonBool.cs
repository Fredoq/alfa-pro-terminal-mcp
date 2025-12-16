using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a boolean value from a JSON node by property name. Usage example: bool liquid = new JsonBool(node, "IsLiquid").Value().
/// </summary>
public sealed record JsonBool(JsonElement Node, string Name) : IJsonValue<bool>
{
    /// <summary>
    /// Returns the boolean value extracted from the JSON node. Usage example: bool ok = new JsonBool(node, "Enabled").Value().
    /// </summary>
    public bool Value()
    {
        ArgumentException.ThrowIfNullOrEmpty(Name);
        if (!Node.TryGetProperty(Name, out JsonElement value))
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        if (value.ValueKind != JsonValueKind.True && value.ValueKind != JsonValueKind.False)
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        return value.GetBoolean();
    }
}
