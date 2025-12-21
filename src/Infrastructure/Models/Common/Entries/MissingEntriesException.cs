namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

/// <summary>
/// Indicates that the required JSON array is missing in the payload. Usage example: throw new MissingEntriesException("Response data array is missing").
/// </summary>
internal sealed class MissingEntriesException : InvalidOperationException
{
    /// <summary>
    /// Creates a missing entries exception. Usage example: throw new MissingEntriesException().
    /// </summary>
    public MissingEntriesException()
    {
    }

    /// <summary>
    /// Creates a missing entries exception with a message. Usage example: throw new MissingEntriesException("Response data array is missing").
    /// </summary>
    /// <param name="text">Error message.</param>
    public MissingEntriesException(string text) : base(text)
    {
    }

    /// <summary>
    /// Creates a missing entries exception with a message and inner exception. Usage example: throw new MissingEntriesException("Response data array is missing", inner).
    /// </summary>
    /// <param name="text">Error message.</param>
    /// <param name="inner">Inner exception.</param>
    public MissingEntriesException(string text, Exception inner) : base(text, inner)
    {
    }
}
