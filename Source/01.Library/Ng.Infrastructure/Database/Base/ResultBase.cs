using Dapper;

namespace Ng.Infrastructure.Database.Base;

public class QueryResultBase : IDisposable, IAsyncDisposable
{
    public QueryResult ResultCode { get; set; } = QueryResult.None;

    public virtual async Task ReadResultsAsync(SqlMapper.GridReader reader)
    {
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}

//.//

public class QueryResultList<T1> : QueryResultBase
{
    public List<T1> Values1 { get; private set; } = new();

    public async Task ReadResultsAsync(IEnumerable<T1> list)
    {
        await Task.CompletedTask;
        Values1 = list.ToList();
    }

    public override async Task ReadResultsAsync(SqlMapper.GridReader reader)
    {
        Values1 = (await reader.ReadAsync<T1>()).ToList();
    }
}

//.//

public class QueryResultList<T1, T2> : QueryResultList<T1>
{
    public List<T2> Values2 { get; private set; } = new();

    public override async Task ReadResultsAsync(SqlMapper.GridReader reader)
    {
        await base.ReadResultsAsync(reader);
        Values2 = (await reader.ReadAsync<T2>()).ToList();
    }
}

//.//

public class QueryResultList<T1, T2, T3> : QueryResultList<T1, T2>
{
    public List<T3> Values3 { get; private set; } = new();

    public override async Task ReadResultsAsync(SqlMapper.GridReader reader)
    {
        await base.ReadResultsAsync(reader);
        Values3 = (await reader.ReadAsync<T3>()).ToList();
    }

}

//.//

public class QueryResultList<T1, T2, T3, T4> : QueryResultList<T1, T2, T3>
{
    public List<T4> Values4 { get; private set; } = new();

    public override async Task ReadResultsAsync(SqlMapper.GridReader reader)
    {
        await base.ReadResultsAsync(reader);
        Values4 = (await reader.ReadAsync<T4>()).ToList();
    }
}
