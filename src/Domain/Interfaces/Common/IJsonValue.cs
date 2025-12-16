namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;

/// <summary>
/// Represents a typed value reader. Usage example: long id = new JsonInteger(node, "IdObject").Value().
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public interface IJsonValue<out T> where T : notnull
{
    /// <summary>
    /// Returns the typed value extracted from the JSON node. Usage example: T value = reader.Value().
    /// </summary>
    T Value();
}
