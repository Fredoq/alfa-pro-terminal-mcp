using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Provides tool lookup and listing. Usage example: ICatalog catalog = new Catalog(items).
/// </summary>
internal sealed class Catalog : ICatalog
{
    private readonly IReadOnlyList<IMcpTool> _items;

    /// <summary>
    /// Creates a catalog from the provided tools. Usage example: ICatalog catalog = new Catalog(items).
    /// </summary>
    /// <param name="items">Tool collection.</param>
    public Catalog(IReadOnlyList<IMcpTool> items)
    {
        _items = items;
    }

    /// <summary>
    /// Returns list tools response. Usage example: ListToolsResult list = await catalog.Tools(request, token).
    /// </summary>
    public ValueTask<ListToolsResult> Tools(RequestContext<ListToolsRequestParams> request, CancellationToken token)
    {
        List<Tool> list = [];
        foreach (IMcpTool item in _items)
        {
            list.Add(item.Tool());
        }
        return new ValueTask<ListToolsResult>(new ListToolsResult { Tools = list });
    }

    /// <summary>
    /// Returns a tool by name. Usage example: IMcpTool tool = catalog.Tool(name).
    /// </summary>
    public IMcpTool Tool(string name)
    {
        IMcpTool item = _items.FirstOrDefault(tool => tool.Name() == name) ?? throw new McpProtocolException($"Unknown tool: '{name}'", McpErrorCode.InvalidRequest);
        return item;
    }
}
