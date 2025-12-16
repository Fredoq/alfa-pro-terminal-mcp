using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a 64-bit integer value from a JSON node by property name. Usage example: long id = new JsonInteger(node, "IdObject").Value().
/// </summary>
public sealed record JsonInteger(JsonElement Node, string Name) : IJsonValue<long>
{
    /// <summary>
    /// Returns the integer value extracted from the JSON node. Usage example: long id = new JsonInteger(node, "IdObject").Value().
    /// </summary>
    public long Value()
    {
        ArgumentException.ThrowIfNullOrEmpty(Name);
        if (!Node.TryGetProperty(Name, out JsonElement value))
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        if (value.ValueKind != JsonValueKind.Number)
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        return value.GetInt64();
    }
}
