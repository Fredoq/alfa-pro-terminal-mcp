using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Provides a typed JSON value object for a JSON element and property name. Usage example: long value = item.Value(node, "Id").Value().
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
internal interface IJsonItem<out T> where T : notnull
{
    /// <summary>
    /// Returns a typed value object bound to the provided JSON element. Usage example: IJsonValue&lt;T&gt; value = item.Value(node, name).
    /// </summary>
    /// <param name="node">Source JSON element.</param>
    /// <param name="name">Source JSON property name.</param>
    IJsonValue<T> Value(JsonElement node, string name);
}
