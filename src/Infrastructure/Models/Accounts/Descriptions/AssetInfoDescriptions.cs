using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Descriptions;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;

/// <summary>
/// Provides description texts for asset info outputs. Usage example: string text = new AssetInfoDescriptions().Text("Ticker").
/// </summary>
internal sealed class AssetInfoDescriptions : IDescriptions
{
    private readonly Dictionary<string, string> _data = new Dictionary<string, string>
    {
        ["IdObject"] = "Asset identifier",
        ["Ticker"] = "Exchange ticker",
        ["ISIN"] = "International security identifier",
        ["Name"] = "Asset name",
        ["Description"] = "Asset description",
        ["Nominal"] = "Nominal value",
        ["IdObjectType"] = "Asset type identifier",
        ["IdObjectGroup"] = "Asset group identifier",
        ["IdObjectBase"] = "Base asset identifier",
        ["IdObjectFaceUnit"] = "Face value currency identifier",
        ["MatDateObject"] = "Expiration date of asset",
        ["IdFi"] = "Financial instrument identifier",
        ["RCode"] = "Portfolio code",
        ["IsLiquid"] = "Liquidity flag",
        ["IdMarketBoard"] = "Market identifier"
    };

    /// <summary>
    /// Returns a description text for the output field. Usage example: string text = descriptions.Text("IdObject").
    /// </summary>
    public string Text(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        if (_data.TryGetValue(name, out string? text))
        {
            return text;
        }
        throw new InvalidOperationException($"{name} description is missing");
    }
}
