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
    private readonly IReadOnlyDictionary<string, JsonElement> _extra;

    /// <summary>
    /// Creates mapped payload using the input schema. Usage example: var payload = new MappedPayload(data, schema).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <param name="schema">Input schema validator.</param>
    public MappedPayload(IReadOnlyDictionary<string, JsonElement> data, IInputSchema schema) : this(data, schema, new Dictionary<string, JsonElement>(StringComparer.Ordinal))
    {
    }

    /// <summary>
    /// Creates mapped payload using the input schema and extra values. Usage example: var payload = new MappedPayload(data, schema, extra).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <param name="schema">Input schema validator.</param>
    /// <param name="extra">Extra argument dictionary.</param>
    public MappedPayload(IReadOnlyDictionary<string, JsonElement> data, IInputSchema schema, IReadOnlyDictionary<string, JsonElement> extra)
    {
        ArgumentNullException.ThrowIfNull(_data);
        ArgumentNullException.ThrowIfNull(_schema);
        ArgumentNullException.ThrowIfNull(_extra);
        _data = data;
        _schema = schema;
        _extra = extra;
    }

    /// <summary>
    /// Serializes payload into transport format. Usage example: string json = payload.AsString();.
    /// </summary>
    /// <returns>Serialized payload.</returns>
    public string AsString()
    {
        _schema.Ensure(_data);
        Dictionary<string, JsonElement> map = new(_data.Count + _extra.Count, StringComparer.Ordinal);
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
            map[key] = pair.Value;
        }
        foreach (KeyValuePair<string, JsonElement> pair in _extra)
        {
            string name = pair.Key;
            if (name.Length == 0)
            {
                throw new McpProtocolException("Argument name is empty", McpErrorCode.InvalidParams);
            }
            char head = char.ToUpperInvariant(name[0]);
            string tail = name.Length > 1 ? name[1..] : string.Empty;
            string key = string.Concat(head, tail);
            map[key] = pair.Value;
        }
        return JsonSerializer.Serialize(map);
    }
}
