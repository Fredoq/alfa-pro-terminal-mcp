using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Trading;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Transport;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Routing;
using Microsoft.Extensions.Logging;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Terminal;

/// <summary>
/// Resolves subaccount identifiers through terminal data sources. Usage example: long id = await new WsSubaccount(terminal, log).Identifier(account, token).
/// </summary>
public sealed class WsSubaccount : ISubaccount
{
    private readonly WsClientSubAccounts _source;

    /// <summary>
    /// Creates a subaccount resolver bound to the terminal. Usage example: var resolver = new WsSubaccount(terminal, log).
    /// </summary>
    /// <param name="terminal">Terminal connection.</param>
    /// <param name="log">Logger instance.</param>
    public WsSubaccount(ITerminal terminal, ILogger log)
    {
        _source = new WsClientSubAccounts(terminal, log);
    }

    /// <summary>
    /// Returns a subaccount identifier for the specified account. Usage example: long id = await resolver.Identifier(account, token).
    /// </summary>
    /// <param name="account">Account identifier.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Subaccount identifier.</returns>
    public async Task<long> Identifier(long account, CancellationToken token = default)
    {
        IEntries entries = await _source.Entries(new EntityPayload("ClientSubAccountEntity", true), token);
        JsonObject root = entries.StructuredContent().AsObject();
        if (!root.TryGetPropertyValue("clientSubAccounts", out JsonNode? data) || data is null)
        {
            throw new InvalidOperationException("Client subaccounts are missing");
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
            if (flag)
            {
                throw new InvalidOperationException("Multiple client subaccounts are matched");
            }
            value = new JsonInteger(node, "IdSubAccount").Value();
            flag = true;
        }
        if (!flag)
        {
            throw new InvalidOperationException("Client subaccount is missing");
        }
        return value;
    }
}
