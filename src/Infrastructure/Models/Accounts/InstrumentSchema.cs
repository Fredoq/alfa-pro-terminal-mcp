using System.Text.Json;
using System.Text.Json.Nodes;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Common;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts;

/// <summary>
/// Defines an output schema for an instrument entry inside asset info payload. Usage example: JsonNode node = new InstrumentSchema().Node(element).
/// </summary>
internal sealed class InstrumentSchema : IJsonSchema
{
    /// <summary>
    /// Returns an output node for the instrument element. Usage example: JsonNode node = schema.Node(element).
    /// </summary>
    public JsonNode Node(JsonElement node)
    {
        AssetInfoDescriptions text = new();
        RulesSchema schema = new([
            new ValueRule<long>(new JsonInteger(node, "IdFi"), "IdFi", text.Text("IdFi")),
            new ValueRule<string>(new JsonString(node, "RCode"), "RCode", text.Text("RCode")),
            new ValueRule<bool>(new JsonBool(node, "IsLiquid"), "IsLiquid", text.Text("IsLiquid")),
            new ValueRule<long>(new JsonInteger(node, "IdMarketBoard"), "IdMarketBoard", text.Text("IdMarketBoard"))
        ]);
        return schema.Node(node);
    }
}
