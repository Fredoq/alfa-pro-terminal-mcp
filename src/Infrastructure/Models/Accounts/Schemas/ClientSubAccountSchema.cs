using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output schema for client subaccount entries. Usage example: JsonNode node = new ClientSubAccountSchema().Node(item).
/// </summary>
internal sealed class ClientSubAccountSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates a client subaccount schema with fields. Usage example: var schema = new ClientSubAccountSchema().
    /// </summary>
    public ClientSubAccountSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdSubAccount"),
            new WholeRule("IdAccount")
        ]);
    }

    /// <summary>
    /// Returns an output node for the client subaccount element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    /// <param name="node">Source JSON object.</param>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
