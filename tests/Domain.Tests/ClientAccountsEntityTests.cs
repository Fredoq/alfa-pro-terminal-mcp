using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Text.Json;

/// <summary>
/// Validates ClientAccountsEntity payload shape. Usage example: executed by xUnit runner.
/// </summary>
public sealed class ClientAccountsEntityTests
{
    /// <summary>
    /// Checks that ClientAccountsEntity produces initialized payload. Usage example: new ClientAccountsEntity().AsString().
    /// </summary>
    [Fact(DisplayName = "ClientAccountsEntity produces initialized payload")]
    public void Given_entity_when_serialized_then_init_is_true()
    {
        ClientAccountsEntity entity = new();
        string json = entity.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        bool result = root.GetProperty("Type").GetString() == "ClientAccountEntity" && root.GetProperty("Init").GetBoolean();
        Assert.True(result, "ClientAccountsEntity does not produce initialized payload");
    }
}
