using System.Text.Json;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Provides JsonElement helpers for strict property access.
/// Usage example: using JsonDocument doc = JsonDocument.Parse(json); string id = doc.RootElement.String("Id");.
/// </summary>
internal static class JsonElementExtensions
{
    extension(JsonElement element)
    {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        /// <summary>
        /// Returns a required string property.
        /// Usage example: string channel = element.String("Channel").
        /// </summary>
        /// <param name="name">Json property name</param>
        public string String(string name)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if (element.TryGetProperty(name, out JsonElement value))
            {
                switch (value.ValueKind)
                {
                    case JsonValueKind.String:
                        return value.GetString() ?? throw new InvalidOperationException($"Property '{name}' is null");
                    case JsonValueKind.Null:
                        return string.Empty;
                }
            }
            throw new InvalidOperationException($"Property '{name}' is missing or not a string");
        }

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        /// <summary>
        /// Returns a required numeric property.
        /// Usage example: int value = element.Number("Data").
        /// </summary>
        /// <param name="name">Json property name</param>
        public int Number(string name)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if (element.TryGetProperty(name, out JsonElement value) && value.ValueKind == JsonValueKind.Number)
            {
                return value.GetInt32();
            }
            throw new InvalidOperationException($"Property '{name}' is missing or not a number");
        }
    }
}
