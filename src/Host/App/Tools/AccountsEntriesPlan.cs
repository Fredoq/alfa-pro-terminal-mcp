using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Models.Accounts;
using Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Inputs;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Provides tool metadata and payload for account entries. Usage example: Tool tool = plan.Tool().
/// </summary>
internal sealed class AccountsEntriesPlan : IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    public Tool Tool()
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        JsonElement input = schema.Schema();
        JsonElement output = JsonSerializer.Deserialize<JsonElement>("""{"type":"object","properties":{"accounts":{"type":"array","description":"List of brokerage accounts available to the user","items":{"type":"object","properties":{"AccountId":{"type":"integer","description":"Unique identifier of the brokerage account used to reference the account in subsequent operations"},"IIAType":{"type":"integer","enum":[0,1,2],"description":"Individual Investment Account type code Values: 0 standard account, 1 IIA Type A, 2 IIA Type B"}},"required":["AccountId","IIAType"],"additionalProperties":false}}},"required":["accounts"],"additionalProperties":false}""");
        return new Tool
        {
            Name = "entries",
            Title = "Accounts entries",
            Description = "Returns a collection of client accounts. Each account contains an identifier and IIA type.",
            InputSchema = input,
            OutputSchema = output,
            Annotations = new ToolAnnotations
            {
                ReadOnlyHint = true,
                IdempotentHint = true,
                OpenWorldHint = false,
                DestructiveHint = false
            }
        };
    }

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    public IPayload Payload(IReadOnlyDictionary<string, JsonElement> data)
    {
        InputSchema schema = new InputSchema(JsonSerializer.Deserialize<JsonElement>("""{"type":"object"}"""));
        schema.Ensure(data);
        return new ClientAccountsEntity();
    }
}
