using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for an instrument entry inside asset info payload. Usage example: JsonNode node = new InstrumentSchema().Node(item).
/// </summary>
internal sealed class InstrumentSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an instrument schema with fields. Usage example: var schema = new InstrumentSchema().
    /// </summary>
    public InstrumentSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdFi"),
            new TextRule("RCode"),
            new FlagRule("IsLiquid"),
            new WholeRule("IdMarketBoard")
        ]);
    }

    /// <summary>
    /// Returns an output node for the instrument element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
