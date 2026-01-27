namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Trading;

/// <summary>
/// Holds instrument identifiers and code for trading. Usage example: var item = new InstrumentValue(group, market, code).
/// </summary>
/// <param name="Group">Object group identifier.</param>
/// <param name="Market">Market board identifier.</param>
/// <param name="Code">Portfolio code.</param>
public sealed record InstrumentValue(long Group, long Market, string Code);
