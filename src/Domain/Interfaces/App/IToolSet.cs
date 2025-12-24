using System.Collections.Generic;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines tool collection retrieval. Usage example: IReadOnlyList&lt;IMcpTool&gt; list = item.Tools().
/// </summary>
public interface IToolSet
{
    /// <summary>
    /// Returns tool collection. Usage example: IReadOnlyList&lt;IMcpTool&gt; list = item.Tools().
    /// </summary>
    IReadOnlyList<IMcpTool> Tools();
}
