using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;

/// <summary>
/// Maps input data to PascalCase JSON payload. Usage example: string json = new MappedPayload(data, schema).AsString().
/// </summary>
internal sealed class MappedPayload : IPayload
{
    private readonly IReadOnlyDictionary<string, JsonElement> _data;
    private readonly IInputSchema _schema;

    /// <summary>
    /// Creates mapped payload using the input schema. Usage example: var payload = new MappedPayload(data, schema).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <param name="schema">Input schema validator.</param>
    public MappedPayload(IReadOnlyDictionary<string, JsonElement> data, IInputSchema schema)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(schema);
        _data = data;
        _schema = schema;
    }

    /// <summary>
    /// Serializes payload into transport format. Usage example: string json = payload.AsString();.
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString()
    {
        _schema.Ensure(_data);
        Dictionary<string, JsonElement> map = new(_data.Count, StringComparer.Ordinal);
        foreach (KeyValuePair<string, JsonElement> pair in _data)
        {
            string name = pair.Key;
            if (name.Length == 0)
            {
                throw new McpProtocolException("Argument name is empty", McpErrorCode.InvalidParams);
            }
            char head = char.ToUpperInvariant(name[0]);
            string tail = name.Length > 1 ? name[1..] : string.Empty;
            string key = string.Concat(head, tail);
            map.Add(key, pair.Value);
        }
        return JsonSerializer.Serialize(map);
    }
}
