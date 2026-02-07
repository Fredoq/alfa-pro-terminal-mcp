using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text.Json;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Filters;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Accounts.Schemas;
using Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Models.Common.Entries;

namespace Fredoqw.Alfa.ProTerminal.Mcp.Infrastructure.Tests;

/// <summary>
/// Verifies allowed order parameters entries filtering. Usage example: executed by xUnit runner.
/// </summary>
public sealed class AllowedOrderParamsEntriesTests
{
    /// <summary>
    /// Ensures that allowed order parameters entries only return matching payloads. Usage example: new SchemaEntries(...).StructuredContent().
    /// </summary>
    [Fact(DisplayName = "Allowed order parameters entries return only matching payloads")]
    public void Allowed_order_parameters_entries_return_only_matching_payloads()
    {
        long param = RandomNumberGenerator.GetInt32(10_000, 99_999);
        long group = RandomNumberGenerator.GetInt32(1, 100_000);
        long board = RandomNumberGenerator.GetInt32(1, 100_000);
        long order = RandomNumberGenerator.GetInt32(1, 3);
        long life = RandomNumberGenerator.GetInt32(0, 2) == 0 ? 5 : 9;
        long kind = RandomNumberGenerator.GetInt32(1, 2);
        long quantity = RandomNumberGenerator.GetInt32(1, 2);
        long price = RandomNumberGenerator.GetInt32(1, 2);
        long other = param + RandomNumberGenerator.GetInt32(1, 100);
        string note = $"café-{param}";
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdAllowedOrderParams = param,
                    IdObjectGroup = group,
                    IdMarketBoard = board,
                    IdOrderType = order,
                    IdDocumentType = kind,
                    IdQuantityType = quantity,
                    IdPriceType = price,
                    IdLifeTime = life,
                    IdExecutionType = RandomNumberGenerator.GetInt32(1, 10),
                    Note = note
                },
                new
                {
                    IdAllowedOrderParams = other,
                    IdObjectGroup = group + 1,
                    IdMarketBoard = board + 1,
                    IdOrderType = order == 1 ? 2 : 1,
                    IdDocumentType = kind,
                    IdQuantityType = quantity,
                    IdPriceType = price,
                    IdLifeTime = life == 5 ? 9 : 5,
                    IdExecutionType = RandomNumberGenerator.GetInt32(1, 10),
                    Note = note
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AllowedOrderParamsScope(group, board, order, life), "Allowed order parameters are missing"), new AllowedOrderParamSchema());
        string json = entries.StructuredContent().ToJsonString();
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        bool match = root.GetArrayLength() == 1 && root[0].GetProperty("IdAllowedOrderParams").GetInt64() == param;
        Assert.True(match, "Allowed order parameters entries do not filter payloads");
    }

    /// <summary>
    /// Checks that allowed order parameters entries remain consistent under concurrency. Usage example: entries.StructuredContent().
    /// </summary>
    [Fact(DisplayName = "Allowed order parameters entries remain consistent under concurrency")]
    public void Allowed_order_parameters_entries_remain_consistent_under_concurrency()
    {
        long param = RandomNumberGenerator.GetInt32(100_000, 199_999);
        long group = RandomNumberGenerator.GetInt32(1, 100_000);
        long board = RandomNumberGenerator.GetInt32(1, 100_000);
        long order = RandomNumberGenerator.GetInt32(1, 3);
        long life = RandomNumberGenerator.GetInt32(0, 2) == 0 ? 5 : 9;
        long kind = RandomNumberGenerator.GetInt32(1, 2);
        long quantity = RandomNumberGenerator.GetInt32(1, 2);
        long price = RandomNumberGenerator.GetInt32(1, 2);
        string note = $"naïve-{group}";
        string payload = JsonSerializer.Serialize(new
        {
            Data = new object[]
            {
                new
                {
                    IdAllowedOrderParams = param,
                    IdObjectGroup = group,
                    IdMarketBoard = board,
                    IdOrderType = order,
                    IdDocumentType = kind,
                    IdQuantityType = quantity,
                    IdPriceType = price,
                    IdLifeTime = life,
                    IdExecutionType = RandomNumberGenerator.GetInt32(1, 10),
                    Note = note
                }
            }
        });
        SchemaEntries entries = new(new FilteredEntries(new PayloadArrayEntries(payload), new AllowedOrderParamsScope(group, board, order, life), "Allowed order parameters are missing"), new AllowedOrderParamSchema());
        ConcurrentBag<string> results = [];
        Parallel.For(0, 4, _ => results.Add(entries.StructuredContent().ToJsonString()));
        string sample = results.First();
        bool match = results.All(item => item == sample);
        Assert.True(match, "Allowed order parameters entries do not remain consistent under concurrency");
    }
}
