using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Text.Json;

/// <summary>
/// Validates ClientPositionEntity payload shape. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ClientPositionEntityTests
{
    /// <summary>
    /// Checks that ClientPositionEntity produces initialized payload. Usage example: new ClientPositionEntity().AsString().
    /// </summary>
    [Fact(DisplayName = "ClientPositionEntity produces initialized payload")]
    public void Given_entity_when_serialized_then_init_is_true()
    {
        ClientPositionEntity entity = new();
        string json = entity.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        bool result = root.GetProperty("Type").GetString() == "ClientPositionEntity" && root.GetProperty("Init").GetBoolean();
        Assert.True(result, "ClientPositionEntity does not produce initialized payload");
    }
}
