using System.Threading.Tasks;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines asynchronous run behavior. Usage example: await run.Run().
/// </summary>
public interface IRun
{
    /// <summary>
    /// Runs the workflow. Usage example: await run.Run().
    /// </summary>
    Task Run();
}
