using System.Threading;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Host.App;

/// <summary>
/// Defines console signal behavior. Usage example: ISignal signal = new Signal(source).
/// </summary>
internal interface ISignal
{
    /// <summary>
    /// Hooks the signal. Usage example: signal.Run().
    /// </summary>
    void Run();
}

/// <summary>
/// Defines token retrieval behavior. Usage example: CancellationToken token = token.Token().
/// </summary>
internal interface IToken
{
    /// <summary>
    /// Returns cancellation token. Usage example: CancellationToken token = token.Token().
    /// </summary>
    CancellationToken Token();
}

/// <summary>
/// Hooks console cancellation and provides a token. Usage example: ISignal signal = new Signal(source).
/// </summary>
internal sealed class Signal : ISignal, IToken
{
    private readonly CancellationTokenSource _source;

    /// <summary>
    /// Creates console signal wrapper. Usage example: ISignal signal = new Signal(source).
    /// </summary>
    /// <param name="source">Cancellation source.</param>
    public Signal(CancellationTokenSource source)
    {
        _source = source;
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
