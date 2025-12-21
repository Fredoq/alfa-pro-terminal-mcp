using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Rules;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Schemas;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;

/// <summary>
/// Defines an output schema for a single asset info entry. Usage example: JsonNode node = new AssetInfoSchema().Node(element).
/// </summary>
internal sealed class AssetInfoSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an asset info schema with described fields. Usage example: var schema = new AssetInfoSchema().
    /// </summary>
    public AssetInfoSchema()
    {
        AssetInfoDescriptions text = new();
        _schema = new RulesSchema([
            new WholeRule("IdObject", text.Text("IdObject")),
            new TextRule("Ticker", text.Text("Ticker")),
            new TextRule("ISIN", text.Text("ISIN")),
            new TextRule("Name", text.Text("Name")),
            new TextRule("Description", text.Text("Description")),
            new RealRule("Nominal", text.Text("Nominal")),
            new WholeRule("IdObjectType", text.Text("IdObjectType")),
            new WholeRule("IdObjectGroup", text.Text("IdObjectGroup")),
            new WholeRule("IdObjectBase", text.Text("IdObjectBase")),
            new WholeRule("IdObjectFaceUnit", text.Text("IdObjectFaceUnit")),
            new TextRule("MatDateObject", text.Text("MatDateObject")),
            new ArrayRule("Instruments", new InstrumentSchema())
        ]);
    }

    /// <summary>
    /// Returns an output node for the asset element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
