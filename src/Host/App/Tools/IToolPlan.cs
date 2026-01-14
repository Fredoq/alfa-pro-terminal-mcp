using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;
using ModelContextProtocol.Protocol;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Tools;

/// <summary>
/// Defines tool metadata and payload building. Usage example: Tool tool = plan.Tool(); IPayload payload = plan.Payload(data).
/// </summary>
internal interface IToolPlan
{
    /// <summary>
    /// Returns tool metadata with schemas and annotations. Usage example: Tool tool = plan.Tool().
    /// </summary>
    Tool Tool();

    /// <summary>
    /// Builds payload for the provided arguments. Usage example: IPayload payload = plan.Payload(data).
    /// </summary>
    /// <param name="data">Input argument dictionary.</param>
    /// <returns>Payload instance.</returns>
    IPayload Payload(IReadOnlyDictionary<string, JsonElement> data);
}
