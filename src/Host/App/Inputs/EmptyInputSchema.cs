using System.Text.Json;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

/// <summary>
/// Represents an empty input schema that rejects any arguments. Usage example: var schema = new EmptyInputSchema(json); schema.Ensure(data).
/// </summary>
internal sealed class EmptyInputSchema : IInputSchema
{
    private readonly JsonElement _schema;

    /// <summary>
    /// Creates empty input schema validator. Usage example: var schema = new EmptyInputSchema(json).
    /// </summary>
    /// <param name="schema">Schema to validate inputs.</param>
    public EmptyInputSchema(JsonElement schema)
    {
        _schema = schema;
    }

    /// <summary>
    /// Returns the input schema. Usage example: JsonElement schema = item.Schema().
    /// </summary>
    /// <returns>Input schema.</returns>
    public JsonElement Schema() => _schema;

    /// <summary>
    /// Ensures the input data matches the empty schema. Usage example: item.Ensure(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    public void Ensure(IReadOnlyDictionary<string, JsonElement> data)
    {
        ArgumentNullException.ThrowIfNull(data);
        if (data.Count == 0)
        {
            return;
        }
        throw new McpProtocolException("Unexpected arguments are present", McpErrorCode.InvalidParams);
    }
}
