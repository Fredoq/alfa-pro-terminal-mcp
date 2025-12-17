using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Defines an output schema for a single asset info entry. Usage example: JsonNode node = new AssetInfoSchema().Node(element).
/// </summary>
internal sealed class AssetInfoSchema : IJsonSchema
{
    /// <summary>
    /// Returns an output node for the asset element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node)
    {
        AssetInfoDescriptions text = new();
        RulesSchema schema = new([
            new ValueRule<long>(new JsonInteger(node, "IdObject"), "IdObject", text.Text("IdObject")),
            new ValueRule<string>(new JsonString(node, "Ticker"), "Ticker", text.Text("Ticker")),
            new ValueRule<string>(new JsonString(node, "ISIN"), "ISIN", text.Text("ISIN")),
            new ValueRule<string>(new JsonString(node, "Name"), "Name", text.Text("Name")),
            new ValueRule<string>(new JsonString(node, "Description"), "Description", text.Text("Description")),
            new ValueRule<double>(new JsonDouble(node, "Nominal"), "Nominal", text.Text("Nominal")),
            new ValueRule<long>(new JsonInteger(node, "IdObjectType"), "IdObjectType", text.Text("IdObjectType")),
            new ValueRule<long>(new JsonInteger(node, "IdObjectGroup"), "IdObjectGroup", text.Text("IdObjectGroup")),
            new ValueRule<long>(new JsonInteger(node, "IdObjectBase"), "IdObjectBase", text.Text("IdObjectBase")),
            new ValueRule<long>(new JsonInteger(node, "IdObjectFaceUnit"), "IdObjectFaceUnit", text.Text("IdObjectFaceUnit")),
            new ValueRule<string>(new JsonString(node, "MatDateObject"), "MatDateObject", text.Text("MatDateObject")),
            new ArrayRule("Instruments", new InstrumentSchema())
        ]);
        return schema.Node(node);
    }
}
