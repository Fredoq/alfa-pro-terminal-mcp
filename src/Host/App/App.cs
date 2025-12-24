using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Composes signal and runnable workflow. Usage example: IRun app = new App(signal, run).
/// </summary>
internal sealed class App : IRun
{
    private readonly ISignal _signal;
    private readonly IRun _run;

    /// <summary>
    /// Creates application flow wrapper. Usage example: IRun app = new App(signal, run).
    /// </summary>
    /// <param name="signal">Signal source.</param>
    /// <param name="run">Runnable workflow.</param>
    public App(ISignal signal, IRun run)
    {
        _signal = signal;
        _run = run;
    }

    /// <summary>
    /// Runs the workflow with signal hookup. Usage example: await app.Run().
    /// </summary>
    public Task Run()
    {
        _signal.Run();
        return _run.Run();
    }
}
