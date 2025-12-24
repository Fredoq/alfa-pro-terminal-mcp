using System.Collections.Generic;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines tool collection retrieval. Usage example: IReadOnlyList&lt;IMcpTool&gt; list = item.Tools().
/// </summary>
internal interface IToolSet
{
    /// <summary>
    /// Returns tool collection. Usage example: IReadOnlyList&lt;IMcpTool&gt; list = item.Tools().
    /// </summary>
    IReadOnlyList<IMcpTool> Tools();
}
