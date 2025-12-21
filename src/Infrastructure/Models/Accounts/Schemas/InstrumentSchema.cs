using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for an instrument entry inside asset info payload. Usage example: JsonNode node = new InstrumentSchema().Node(element).
/// </summary>
internal sealed class InstrumentSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an instrument schema with described fields. Usage example: var schema = new InstrumentSchema().
    /// </summary>
    public InstrumentSchema()
    {
        AssetInfoDescriptions text = new();
        _schema = new RulesSchema([
            new WholeRule("IdFi", text.Text("IdFi")),
            new TextRule("RCode", text.Text("RCode")),
            new FlagRule("IsLiquid", text.Text("IsLiquid")),
            new WholeRule("IdMarketBoard", text.Text("IdMarketBoard"))
        ]);
    }

    /// <summary>
    /// Returns an output node for the instrument element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
