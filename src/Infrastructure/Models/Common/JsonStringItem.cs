using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Creates JsonString value objects for string properties. Usage example: string value = new JsonStringItem().Value(node, "Id").Value().
/// </summary>
internal sealed record JsonStringItem : IJsonItem<string>
{
    /// <summary>
    /// Returns a JsonString value object for the provided property. Usage example: IJsonValue&lt;string&gt; value = item.Value(node, name).
    /// </summary>
    public IJsonValue<string> Value(JsonElement node, string name) => new JsonString(node, name);
}

