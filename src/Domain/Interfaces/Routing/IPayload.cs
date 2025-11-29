namespace Fredoqw.Alfa.ProTerminal.Mcp.Domain.Interfaces.Routing;

public interface IPayload
{
    /// <summary>
    /// Returns the payload as string. Usage example: string str = payload.AsString();.
    /// </summary>
    /// <returns> The payload as a string. </returns>
    string AsString();
}
