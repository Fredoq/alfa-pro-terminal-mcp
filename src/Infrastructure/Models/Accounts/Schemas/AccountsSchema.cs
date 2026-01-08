using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines output for account entries. Usage example: JsonNode node = new AccountsSchema().Node(element).
/// </summary>
internal sealed class AccountsSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an accounts schema with fields. Usage example: var schema = new AccountsSchema().
    /// </summary>
    public AccountsSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("AccountId", "IdAccount"),
            new WholeRule("IIAType")
        ]);
    }

    /// <summary>
    /// Returns an account entry node. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    /// <param name="node">Source JSON element.</param>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
