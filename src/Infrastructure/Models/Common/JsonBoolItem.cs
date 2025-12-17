using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Creates JsonBool value objects for boolean properties. Usage example: bool value = new JsonBoolItem().Value(node, "Flag").Value().
/// </summary>
internal sealed record JsonBoolItem : IJsonItem<bool>
{
    /// <summary>
    /// Returns a JsonBool value object for the provided property. Usage example: IJsonValue&lt;bool&gt; value = item.Value(node, name).
    /// </summary>
    public IJsonValue<bool> Value(JsonElement node, string name) => new JsonBool(node, name);
}

