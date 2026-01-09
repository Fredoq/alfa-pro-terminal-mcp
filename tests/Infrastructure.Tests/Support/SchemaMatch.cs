using System.Text.Json;
using System.Text.Json.Nodes;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests.Support;

/// <summary>
/// Describes schema matching for structured JSON content. Usage example: bool ok = match.Match(node, schema).
/// </summary>
internal interface ISchemaMatch
{
    /// <summary>
    /// Reports whether the node conforms to the schema. Usage example: bool ok = match.Match(node, schema).
    /// </summary>
    bool Match(JsonNode node, JsonElement schema);
}

/// <summary>
/// Matches structured JSON content against an output schema. Usage example: bool ok = new SchemaMatch().Match(node, schema).
/// </summary>
internal sealed class SchemaMatch : ISchemaMatch
{
    /// <summary>
    /// Reports whether the node conforms to the schema. Usage example: bool ok = match.Match(node, schema).
    /// </summary>
    public bool Match(JsonNode node, JsonElement schema)
    {
        if (schema.TryGetProperty("oneOf", out JsonElement group))
        {
            foreach (JsonElement item in group.EnumerateArray())
            {
                if (Match(node, item))
                {
                    return true;
                }
            }
            return false;
        }
        if (!schema.TryGetProperty("type", out JsonElement type))
        {
            return false;
        }
        string name = type.GetString() ?? string.Empty;
        if (name == "object")
        {
            JsonObject root;
            try
            {
                root = node.AsObject();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            if (schema.TryGetProperty("required", out JsonElement require))
            {
                foreach (JsonElement item in require.EnumerateArray())
                {
                    string key = item.GetString() ?? string.Empty;
                    if (key.Length == 0)
                    {
                        return false;
                    }
                    if (!root.TryGetPropertyValue(key, out JsonNode? value))
                    {
                        return false;
                    }
                    if (value == null)
                    {
                        return false;
                    }
                }
            }
            if (schema.TryGetProperty("properties", out JsonElement props))
            {
                foreach (JsonProperty part in props.EnumerateObject())
                {
                    if (!root.TryGetPropertyValue(part.Name, out JsonNode? value))
                    {
                        continue;
                    }
                    if (value == null)
                    {
                        return false;
                    }
                    if (!Match(value, part.Value))
                    {
                        return false;
                    }
                }
            }
            bool extra = true;
            if (schema.TryGetProperty("additionalProperties", out JsonElement add))
            {
                extra = add.ValueKind != JsonValueKind.False;
            }
            if (!extra)
            {
                HashSet<string> set = [];
                if (schema.TryGetProperty("properties", out JsonElement list))
                {
                    foreach (JsonProperty part in list.EnumerateObject())
                    {
                        set.Add(part.Name);
                    }
                }
                foreach (KeyValuePair<string, JsonNode?> pair in root)
                {
                    if (!set.Contains(pair.Key))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        if (name == "array")
        {
            JsonArray array;
            try
            {
                array = node.AsArray();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            if (schema.TryGetProperty("items", out JsonElement item))
            {
                foreach (JsonNode? part in array)
                {
                    if (part == null)
                    {
                        return false;
                    }
                    if (!Match(part, item))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        if (name == "string")
        {
            try
            {
                _ = node.GetValue<string>();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            if (!schema.TryGetProperty("enum", out JsonElement item))
            {
                return true;
            }
            string raw = node.ToJsonString();
            foreach (JsonElement part in item.EnumerateArray())
            {
                if (raw == part.GetRawText())
                {
                    return true;
                }
            }
            return false;
        }
        if (name == "number")
        {
            try
            {
                _ = node.GetValue<double>();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            if (!schema.TryGetProperty("enum", out JsonElement item))
            {
                return true;
            }
            string raw = node.ToJsonString();
            foreach (JsonElement part in item.EnumerateArray())
            {
                if (raw == part.GetRawText())
                {
                    return true;
                }
            }
            return false;
        }
        if (name == "integer")
        {
            try
            {
                _ = node.GetValue<long>();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            if (!schema.TryGetProperty("enum", out JsonElement item))
            {
                return true;
            }
            string raw = node.ToJsonString();
            foreach (JsonElement part in item.EnumerateArray())
            {
                if (raw == part.GetRawText())
                {
                    return true;
                }
            }
            return false;
        }
        if (name == "boolean")
        {
            try
            {
                _ = node.GetValue<bool>();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            if (!schema.TryGetProperty("enum", out JsonElement item))
            {
                return true;
            }
            string raw = node.ToJsonString();
            foreach (JsonElement part in item.EnumerateArray())
            {
                if (raw == part.GetRawText())
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
}
