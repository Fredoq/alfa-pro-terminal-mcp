namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.App;

/// <summary>
/// Defines console signal behavior. Usage example: ISignal signal = new Signal(source).
/// </summary>
public interface ISignal
{
    /// <summary>
    /// Hooks the signal. Usage example: signal.Run().
    /// </summary>
    void Run();
}
