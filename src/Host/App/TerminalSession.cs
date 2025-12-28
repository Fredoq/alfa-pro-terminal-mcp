using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Hosting;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Runs terminal lifecycle around a workflow. Usage example: IRun run = new TerminalSession(terminal, flow, token).
/// </summary>
internal sealed class TerminalSession : IRun
{
    private readonly AlfaProTerminal _terminal;
    private readonly IRun _run;
    private readonly IToken _token;

    /// <summary>
    /// Creates terminal lifecycle wrapper. Usage example: IRun run = new TerminalSession(terminal, flow, token).
    /// </summary>
    /// <param name="terminal">Terminal instance.</param>
    /// <param name="run">Inner workflow.</param>
    /// <param name="token">Token provider.</param>
    public TerminalSession(AlfaProTerminal terminal, IRun run, IToken token)
    {
        _terminal = terminal;
        _run = run;
        _token = token;
    }

    /// <summary>
    /// Runs workflow with terminal lifecycle. Usage example: await run.Run().
    /// </summary>
    public async Task Run()
    {
        await _terminal.StartAsync(_token.Token());
        try
        {
            await _run.Run();
        }
        finally
        {
            await _terminal.StopAsync(CancellationToken.None);
        }
    }

    /// <summary>
    /// Disposes terminal and inner workflow. Usage example: await run.DisposeAsync().
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _terminal.DisposeAsync();
        await _run.DisposeAsync();
    }
}
