using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;

/// <summary>
/// Filters allowed order parameter entries by instrument and order values. Usage example: bool matched = new AllowedOrderParamsScope(group, board, order, life).Filtered(node).
/// </summary>
internal sealed class AllowedOrderParamsScope : IEntriesFilter
{
    private readonly long _group;
    private readonly long _board;
    private readonly long _order;
    private readonly long _life;

    /// <summary>
    /// Creates an allowed order parameters scope. Usage example: var scope = new AllowedOrderParamsScope(group, board, order, life).
    /// </summary>
    /// <param name="group">Target object group identifier.</param>
    /// <param name="board">Target market board identifier.</param>
    /// <param name="order">Target order type identifier.</param>
    /// <param name="life">Target order lifetime identifier.</param>
    public AllowedOrderParamsScope(long group, long board, long order, long life)
    {
        _group = group;
        _board = board;
        _order = order;
        _life = life;
    }

    /// <summary>
    /// Determines whether the node matches the allowed order parameters. Usage example: bool matched = scope.Filtered(node).
    /// </summary>
    /// <param name="node">Allowed order parameter payload element.</param>
    public bool Filtered(JsonObject node)
    {
        long group = new JsonInteger(node, "IdObjectGroup").Value();
        long board = new JsonInteger(node, "IdMarketBoard").Value();
        long order = new JsonInteger(node, "IdOrderType").Value();
        long life = new JsonInteger(node, "IdLifeTime").Value();
        long document = new JsonInteger(node, "IdDocumentType").Value();
        long quantity = new JsonInteger(node, "IdQuantityType").Value();
        long price = new JsonInteger(node, "IdPriceType").Value();
        return group == _group && board == _board && order == _order && life == _life && document == 1 && quantity == 1 && price == 1;
    }
}
