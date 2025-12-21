using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Descriptions;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;

/// <summary>
/// Provides description texts for account outputs. Usage example: string text = new AccountsDescriptions().Text("AccountId").
/// </summary>
internal sealed class AccountsDescriptions : IDescriptions
{
    private readonly Dictionary<string, string> _data = new()
    {
        ["AccountId"] = "Client account id",
        ["IIAType"] = "Individual investment account type"
    };

    /// <summary>
    /// Returns a description text for the output field. Usage example: string text = descriptions.Text("IIAType").
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
