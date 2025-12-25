namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides environment name from variables. Usage example: IEnvironmentName name = new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production").
/// </summary>
internal sealed class EnvironmentName : IEnvironmentName
{
    private readonly string _key;
    private readonly string _alias;
    private readonly string _fallback;

    /// <summary>
    /// Creates environment name wrapper. Usage example: IEnvironmentName name = new EnvironmentName("DOTNET_ENVIRONMENT", "ASPNETCORE_ENVIRONMENT", "Production").
    /// </summary>
    /// <param name="key">Primary environment key.</param>
    /// <param name="alias">Secondary environment key.</param>
    /// <param name="fallback">Fallback value.</param>
    public EnvironmentName(string key, string alias, string fallback)
    {
        _key = key;
        _alias = alias;
        _fallback = fallback;
    }

    /// <summary>
    /// Returns environment name. Usage example: string name = item.Name().
    /// </summary>
    public string Name()
    {
        string text = Environment.GetEnvironmentVariable(_key) ?? string.Empty;
        if (text.Length > 0)
        {
            return text;
        }
        text = Environment.GetEnvironmentVariable(_alias) ?? string.Empty;
        if (text.Length > 0)
        {
            return text;
        }
        if (_fallback.Length == 0)
        {
            throw new InvalidOperationException("Environment name is missing");
        }
        return _fallback;
    }
}
