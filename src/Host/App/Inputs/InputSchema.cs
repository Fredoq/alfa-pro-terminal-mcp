using System.Text.Json;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

/// <summary>
/// Validates input data using the provided schema. Usage example: var schema = new InputSchema(json); schema.Ensure(data).
/// </summary>
internal sealed class InputSchema : IInputSchema
{
    private readonly JsonElement _schema;

    /// <summary>
    /// Creates input schema validator. Usage example: var schema = new InputSchema(json).
    /// </summary>
    /// <param name="schema">Schema to validate inputs.</param>
    public InputSchema(JsonElement schema)
    {
        _schema = schema;
    }

    /// <summary>
    /// Returns the input schema. Usage example: JsonElement schema = item.Schema().
    /// </summary>
    /// <returns>Input schema.</returns>
    public JsonElement Schema() => _schema;

    /// <summary>
    /// Ensures the input data matches the schema. Usage example: item.Ensure(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    public void Ensure(IReadOnlyDictionary<string, JsonElement> data)
    {
        ArgumentNullException.ThrowIfNull(data);
        bool flag = true;
        if (_schema.TryGetProperty("additionalProperties", out JsonElement value))
        {
            try
            {
                flag = value.GetBoolean();
            }
            catch (InvalidOperationException)
            {
                flag = true;
            }
        }
        if (_schema.TryGetProperty("required", out JsonElement list))
        {
            foreach (JsonElement item in list.EnumerateArray())
            {
                string name = item.GetString() ?? throw new McpProtocolException("Required argument name is missing", McpErrorCode.InvalidParams);
                if (!data.ContainsKey(name))
                {
                    throw new McpProtocolException($"Missing required argument {name}", McpErrorCode.InvalidParams);
                }
            }
        }
        if (_schema.TryGetProperty("properties", out JsonElement props))
        {
            if (flag)
            {
                return;
            }
            foreach (string name in data.Keys)
            {
                if (!props.TryGetProperty(name, out _))
                {
                    throw new McpProtocolException($"Unexpected argument {name}", McpErrorCode.InvalidParams);
                }
            }
            return;
        }
        if (flag || data.Count == 0)
        {
            return;
        }
        throw new McpProtocolException("Unexpected arguments are present", McpErrorCode.InvalidParams);
    }
}
