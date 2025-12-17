namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

/// <summary>
/// Represents a typed JSON value reader bound to a JSON node and property name. Usage example: long id = new JsonInteger(node, "IdObject").Value().
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IJsonValue<out T> where T : notnull
{
    /// <summary>
    /// Returns the typed value extracted from the JSON node. Usage example: T value = reader.Value().
    /// </summary>
    T Value();
}
