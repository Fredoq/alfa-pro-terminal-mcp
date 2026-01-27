using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Resolves allowed order parameter identifiers through terminal data sources. Usage example: long id = await new WsAllowedOrderParam(terminal, log).Identifier(group, market, price, token).
/// </summary>
public sealed class WsAllowedOrderParam : IAllowedOrderParam
{
    private readonly WsAllowedOrderParams source;

    /// <summary>
    /// Creates an allowed order parameter resolver bound to the terminal. Usage example: var resolver = new WsAllowedOrderParam(terminal, log).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="log">Logger instance.</param>
    public WsAllowedOrderParam(ITerminal terminal, ILogger log)
    {
        source = new WsAllowedOrderParams(terminal, log);
    }

    /// <summary>
    /// Returns allowed order parameter identifier for the provided scope. Usage example: long id = await resolver.Identifier(group, market, price, token).
    /// </summary>
    /// <param name="group">Object group identifier.</param>
    /// <param name="market">Market board identifier.</param>
    /// <param name="price">Limit price.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Allowed order parameter identifier.</returns>
    public async Task<long> Identifier(long group, long market, double price, CancellationToken token = default)
    {
        IEntries entries = await source.Entries(new EntityPayload("AllowedOrderParamEntity", true), token);
        JsonObject root = entries.StructuredContent().AsObject();
        if (!root.TryGetPropertyValue("allowedOrderParams", out JsonNode? data) || data is null)
        {
            throw new InvalidOperationException("Allowed order params are missing");
        }
        JsonArray list = data.AsArray();
        int order = price > 0 ? 2 : 1;
        int life = 9;
        int document = 1;
        int amount = 1;
        int kind = 1;
        long value = 0;
        bool flag = false;
        foreach (JsonNode? item in list)
        {
            if (item is null)
            {
                throw new InvalidOperationException("Entry node is missing");
            }
            JsonObject node = item.AsObject();
            if (new JsonInteger(node, "IdObjectGroup").Value() != group)
            {
                continue;
            }
            if (new JsonInteger(node, "IdMarketBoard").Value() != market)
            {
                continue;
            }
            if (new JsonInteger(node, "IdOrderType").Value() != order)
            {
                continue;
            }
            if (new JsonInteger(node, "IdDocumentType").Value() != document)
            {
                continue;
            }
            if (new JsonInteger(node, "IdQuantityType").Value() != amount)
            {
                continue;
            }
            if (new JsonInteger(node, "IdPriceType").Value() != kind)
            {
                continue;
            }
            if (new JsonInteger(node, "IdLifeTime").Value() != life)
            {
                continue;
            }
            if (flag)
            {
                throw new InvalidOperationException("Multiple allowed order params are matched");
            }
            value = new JsonInteger(node, "IdAllowedOrderParams").Value();
            flag = true;
        }
        if (!flag)
        {
            throw new InvalidOperationException("Allowed order params are missing");
        }
        return value;
    }
}
