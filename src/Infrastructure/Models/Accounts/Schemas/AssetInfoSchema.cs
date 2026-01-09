using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for a single asset info entry. Usage example: JsonNode node = new AssetInfoSchema().Node(item).
/// </summary>
internal sealed class AssetInfoSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an asset info schema with fields. Usage example: var schema = new AssetInfoSchema().
    /// </summary>
    public AssetInfoSchema()
    {
        _schema = new RulesSchema([
            new WholeRule("IdObject"),
            new TextRule("Ticker"),
            new TextRule("ISIN"),
            new TextRule("Name"),
            new TextRule("Description"),
            new RealRule("Nominal"),
            new WholeRule("IdObjectType"),
            new WholeRule("IdObjectGroup"),
            new WholeRule("IdObjectBase"),
            new WholeRule("IdObjectFaceUnit"),
            new TextRule("MatDateObject"),
            new ArrayRule("Instruments", new InstrumentSchema())
        ]);
    }

    /// <summary>
    /// Returns an output node for the asset element. Usage example: JsonNode node = schema.Node(item).
    /// </summary>
    public JsonNode Node(JsonObject node) => _schema.Node(node);
}
