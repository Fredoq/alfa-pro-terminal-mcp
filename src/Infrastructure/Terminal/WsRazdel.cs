using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Resolves portfolio identifiers for subaccounts. Usage example: long id = await new WsRazdel(terminal, log).Identifier(account, subaccount, code, token).
/// </summary>
public sealed class WsRazdel : IRazdel
{
    private readonly WsSubAccountRazdels source;

    /// <summary>
    /// Creates a portfolio resolver bound to the terminal. Usage example: var resolver = new WsRazdel(terminal, log).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="log">Logger instance.</param>
    public WsRazdel(ITerminal terminal, ILogger log)
    {
        source = new WsSubAccountRazdels(terminal, log);
    }

    /// <summary>
    /// Returns a portfolio identifier for the specified account, subaccount, and code. Usage example: long id = await resolver.Identifier(account, subaccount, code, token).
    /// </summary>
    /// <param name="account">Account identifier.</param>
    /// <param name="subaccount">Subaccount identifier.</param>
    /// <param name="code">Portfolio code.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Portfolio identifier.</returns>
    public async Task<long> Identifier(long account, long subaccount, string code, CancellationToken token = default)
    {
        IEntries entries = await source.Entries(new EntityPayload("SubAccountRazdelEntity", true), token);
        JsonObject root = entries.StructuredContent().AsObject();
        if (!root.TryGetPropertyValue("subAccountRazdels", out JsonNode? data) || data is null)
        {
            throw new InvalidOperationException("Subaccount razdels are missing");
        }
        JsonArray list = data.AsArray();
        long value = 0;
        bool flag = false;
        foreach (JsonNode? item in list)
        {
            if (item is null)
            {
                throw new InvalidOperationException("Entry node is missing");
            }
            JsonObject node = item.AsObject();
            if (new JsonInteger(node, "IdAccount").Value() != account)
            {
                continue;
            }
            if (new JsonInteger(node, "IdSubAccount").Value() != subaccount)
            {
                continue;
            }
            if (new JsonString(node, "RCode").Value() != code)
            {
                continue;
            }
            if (flag)
            {
                throw new InvalidOperationException("Multiple subaccount razdels are matched");
            }
            value = new JsonInteger(node, "IdRazdel").Value();
            flag = true;
        }
        if (!flag)
        {
            throw new InvalidOperationException("Subaccount razdel is missing");
        }
        return value;
    }
}
