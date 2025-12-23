using System.Threading.Tasks;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Disposes a resource after workflow completion. Usage example: IRun run = new Scope(item, flow).
/// </summary>
internal sealed class Scope : IRun
{
    private readonly IDisposable _item;
    private readonly IRun _run;

    /// <summary>
    /// Creates scope wrapper. Usage example: IRun run = new Scope(item, flow).
    /// </summary>
    /// <param name="item">Disposable resource.</param>
    /// <param name="run">Inner workflow.</param>
    public Scope(IDisposable item, IRun run)
    {
        _item = item;
        _run = run;
    }

    /// <summary>
    /// Runs workflow and disposes the resource. Usage example: await run.Run().
    /// </summary>
    public async Task Run()
    {
        try
        {
            await _run.Run();
        }
        finally
        {
            _item.Dispose();
        }
    }
}
