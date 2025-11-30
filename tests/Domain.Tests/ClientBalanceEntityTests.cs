using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Text.Json;

/// <summary>
/// Validates ClientBalanceEntity payload shape. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ClientBalanceEntityTests
{
    /// <summary>
    /// Checks that ClientBalanceEntity produces initialized payload. Usage example: new ClientBalanceEntity().AsString().
    /// </summary>
    [Fact(DisplayName = "ClientBalanceEntity produces initialized payload")]
    public void Given_entity_when_serialized_then_init_is_true()
    {
        ClientBalanceEntity entity = new();
        string json = entity.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        bool result = root.GetProperty("Type").GetString() == "ClientBalanceEntity" && root.GetProperty("Init").GetBoolean();
        Assert.True(result, "ClientBalanceEntity does not produce initialized payload");
    }
}
