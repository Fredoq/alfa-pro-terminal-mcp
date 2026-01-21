using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Defines payload creation for tool execution. Usage example: IPayload payload = plan.Payload(data).
/// </summary>
internal interface IPayloadPlan
{
    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    IPayload Payload(IReadOnlyDictionary<string, JsonElement> data);
}
