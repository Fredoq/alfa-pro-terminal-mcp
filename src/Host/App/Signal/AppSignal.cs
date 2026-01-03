using Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App.Signal;

/// <summary>
/// Hooks console cancellation and provides a token. Usage example: ISignal signal = new AppSignal(source).
/// </summary>
internal sealed class AppSignal : ISignal, IToken
{
    private readonly CancellationTokenSource _source;

    /// <summary>
    /// Creates console signal wrapper. Usage example: ISignal signal = new AppSignal(source).
    /// </summary>
    /// <param name="source">Cancellation source.</param>
    public AppSignal(CancellationTokenSource source)
    {
        _source = source;
    }

    /// <summary>
    /// Creates console signal wrapper with new cancellation source. Usage example: ISignal signal = new AppSignal().
    /// </summary>
    public AppSignal() : this(new CancellationTokenSource())
    {
    }

    /// <summary>
    /// Hooks the signal. Usage example: signal.Run().
    /// </summary>
    public void Run()
    {
        Console.CancelKeyPress += (sender, data) =>
        {
            data.Cancel = true;
            _source.Cancel();
        };
    }

    /// <summary>
    /// Returns cancellation token. Usage example: CancellationToken token = token.Token().
    /// </summary>
    public CancellationToken Token() => _source.Token;
}
