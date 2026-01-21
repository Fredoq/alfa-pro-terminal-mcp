using System.Text.Json;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

/// <summary>
/// Provides input schema and validation. Usage example: item.Ensure(data).
/// </summary>
internal interface IInputSchema
{
    /// <summary>
    /// Returns the input schema. Usage example: JsonElement schema = item.Schema().
    /// </summary>
    /// <returns>Input schema.</returns>
    JsonElement Schema();

    /// <summary>
    /// Ensures the input data matches the schema. Usage example: item.Ensure(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    void Ensure(IReadOnlyDictionary<string, JsonElement> data);
}
