using System.Threading.Tasks;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines asynchronous run behavior. Usage example: await run.Run().
/// </summary>
internal interface IRun
{
    /// <summary>
    /// Runs the workflow. Usage example: await run.Run().
    /// </summary>
    Task Run();
}
