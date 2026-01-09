using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

/// <summary>
/// Reads a double value from a JSON node by property name. Usage example: double nominal = new JsonDouble(node, "Nominal").Value().
/// </summary>
public sealed record JsonDouble(JsonObject Node, string Name) : IJsonValue<double>
{
    /// <summary>
    /// Returns the double value extracted from the JSON node. Usage example: double price = new JsonDouble(node, "Price").Value().
    /// </summary>
    public double Value()
    {
        ArgumentException.ThrowIfNullOrEmpty(Name);
        if (!Node.TryGetPropertyValue(Name, out JsonNode? value) || value is null)
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
        try
        {
            return value.GetValue<double>();
        }
        catch (Exception)
        {
            throw new InvalidOperationException($"{Name} is missing");
        }
    }
}
