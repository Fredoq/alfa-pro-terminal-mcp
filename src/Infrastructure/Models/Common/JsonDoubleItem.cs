using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Creates JsonDouble value objects for numeric properties. Usage example: double value = new JsonDoubleItem().Value(node, "Price").Value().
/// </summary>
internal sealed record JsonDoubleItem : IJsonItem<double>
{
    /// <summary>
    /// Returns a JsonDouble value object for the provided property. Usage example: IJsonValue&lt;double&gt; value = item.Value(node, name).
    /// </summary>
    public IJsonValue<double> Value(JsonElement node, string name) => new JsonDouble(node, name);
}

