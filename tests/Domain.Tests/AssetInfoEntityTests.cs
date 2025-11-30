using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Tests;

using System.Text.Json;

/// <summary>
/// Validates AssetInfoEntity payload shape. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AssetInfoEntityTests
{
    /// <summary>
    /// Checks that AssetInfoEntity produces initialized payload. Usage example: new AssetInfoEntity().AsString().
    /// </summary>
    [Fact(DisplayName = "AssetInfoEntity produces initialized payload")]
    public void Given_entity_when_serialized_then_init_is_true()
    {
        AssetInfoEntity entity = new();
        string json = entity.AsString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        bool result = root.GetProperty("Type").GetString() == "AssetInfoEntity" && root.GetProperty("Init").GetBoolean();
        Assert.True(result, "AssetInfoEntity does not produce initialized payload");
    }
}
