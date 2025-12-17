using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Creates JsonInteger value objects for numeric properties. Usage example: long value = new JsonIntegerItem().Value(node, "Id").Value().
/// </summary>
internal sealed record JsonIntegerItem : IJsonItem<long>
{
    /// <summary>
    /// Returns a JsonInteger value object for the provided property. Usage example: IJsonValue&lt;long&gt; value = item.Value(node, name).
    /// </summary>
    public IJsonValue<long> Value(JsonElement node, string name) => new JsonInteger(node, name);
}

