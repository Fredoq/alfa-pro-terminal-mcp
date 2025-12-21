using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a double value from a JSON node by property name. Usage example: double nominal = new JsonDouble(node, "Nominal").Value().
/// </summary>
public sealed record JsonDouble(JsonElement Node, string Name) : IJsonValue<double>
{
    /// <summary>
    /// Returns the double value extracted from the JSON node. Usage example: double price = new JsonDouble(node, "Price").Value().
    /// </summary>
    public double Value()
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
        return value.GetDouble();
    }
}
