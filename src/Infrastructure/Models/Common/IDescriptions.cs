namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

/// <summary>
/// Provides description text for output fields. Usage example: string text = descriptions.Text("IdObject").
/// </summary>
internal interface IDescriptions
{
    /// <summary>
    /// Returns a non-empty description for the provided field name. Usage example: string text = descriptions.Text("Ticker").
    /// </summary>
    /// <param name="name">Output field name.</param>
    string Text(string name);
}

