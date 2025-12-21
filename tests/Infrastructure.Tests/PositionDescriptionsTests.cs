using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Descriptions;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies position description lookup under concurrency. Usage example: executed by xUnit runner.
/// </summary>
public sealed class PositionDescriptionsTests
{
    /// <summary>
    /// Ensures that unknown names are rejected under concurrency. Usage example: new PositionDescriptions().Text(name).
    /// </summary>
    [Fact(DisplayName = "Position descriptions reject unknown names under concurrency")]
    public void Position_descriptions_reject_unknown_names_under_concurrency()
    {
        string name = $"π{RandomNumberGenerator.GetInt32(400, 700)}";
        PositionDescriptions descriptions = new();
        int count = RandomNumberGenerator.GetInt32(2, 6);
        int attempt = 0;
        bool match = false;
        while (attempt < 3 && !match)
        {
            attempt++;
            int seen = 0;
            Parallel.For(0, count, _ =>
            {
                try
                {
                    descriptions.Text(name);
                }
                catch (InvalidOperationException)
                {
                    Interlocked.Increment(ref seen);
                }
            });
            match = seen == count;
        }
        Assert.True(match, "Position descriptions do not reject unknown names under concurrency");
    }
}
