using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Defines an output schema for a single asset info entry. Usage example: JsonNode node = new AssetInfoSchema().Node(element).
/// </summary>
internal sealed class AssetInfoSchema : IJsonSchema
{
    private readonly RulesSchema _schema;

    /// <summary>
    /// Creates an asset info schema with nested instruments. Usage example: var schema = new AssetInfoSchema().
    /// </summary>
    public AssetInfoSchema()
    {
        AssetInfoDescriptions text = new();
        JsonIntegerItem integer = new();
        JsonStringItem stringy = new();
        JsonDoubleItem doubley = new();
        _schema = new RulesSchema([
            new ValueRule<long>(integer, "IdObject", text.Text("IdObject")),
            new ValueRule<string>(stringy, "Ticker", text.Text("Ticker")),
            new ValueRule<string>(stringy, "ISIN", text.Text("ISIN")),
            new ValueRule<string>(stringy, "Name", text.Text("Name")),
            new ValueRule<string>(stringy, "Description", text.Text("Description")),
            new ValueRule<double>(doubley, "Nominal", text.Text("Nominal")),
            new ValueRule<long>(integer, "IdObjectType", text.Text("IdObjectType")),
            new ValueRule<long>(integer, "IdObjectGroup", text.Text("IdObjectGroup")),
            new ValueRule<long>(integer, "IdObjectBase", text.Text("IdObjectBase")),
            new ValueRule<long>(integer, "IdObjectFaceUnit", text.Text("IdObjectFaceUnit")),
            new ValueRule<string>(stringy, "MatDateObject", text.Text("MatDateObject")),
            new ArrayRule("Instruments", new InstrumentSchema())
        ]);
    }

    /// <summary>
    /// Returns an output node for the asset element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node) => _schema.Node(node);
}
