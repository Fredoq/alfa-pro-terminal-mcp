using System.Threading;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines token retrieval behavior. Usage example: CancellationToken token = token.Token().
/// </summary>
public interface IToken
{
    /// <summary>
    /// Returns cancellation token. Usage example: CancellationToken token = token.Token().
    /// </summary>
    CancellationToken Token();
}
