namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Messaging.Responses;

/// <summary>
/// Verifies JsonElementExtensions strict parsing. Usage example: executed by xUnit runner.
/// </summary>
public sealed class JsonElementExtensionsTests
{
    /// <summary>
    /// Verifies JsonElementExtensions.Number parses numeric property. Usage example: int value = element.Number("Data").
    /// </summary>
    [Fact(DisplayName = "JsonElementExtensions parses numeric property")]
    public void Given_numeric_property_when_parsed_then_returns_number()
    {
        int number = RandomNumberGenerator.GetInt32(1, 9);
        using JsonDocument document = JsonDocument.Parse($"{{\"Data\":{number}}}");
        JsonElement element = document.RootElement;
        int value = element.Number("Data");
        Assert.True(value == number, "JsonElementExtensions does not parse numeric property");
    }
}
