using NLog;
using Dapper;

namespace Ng.Infrastructure.Database.Base;

public abstract class ProcedureBase(RepositoryBase repository)
{
    protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    protected RepositoryBase Repository = repository;
    public DynamicParameters Parameters { get; private set; } = new();

    public abstract string ProcedureName { get; }
    public abstract DynamicParameters BuildParameters();

    public virtual bool HasReturnValue() { return false; }
    public abstract int GetReturnValue();

    public async Task<QueryResultBase> ExecuteAsync()
    {
        var result = await Repository.ProcedureExecuteAsync(this);
        if (result.IsError)
        {
            Logger.Error($"{ProcedureName} => {result.Message}");
            return new QueryResultBase { ResultCode = QueryResult.Error };
        }

        return new QueryResultBase { ResultCode = QueryResult.Success };
    }

    public async Task<QueryResultList<T1>> ExecuteAsync<T1>()
    {
        var result = await Repository.ProcedureAsync<T1>(this);
        if (result.IsError)
        {
            Logger.Error($"{ProcedureName} => {result.Message}");
            return new QueryResultList<T1> { ResultCode = QueryResult.Error };
        }

        var queryResult = new QueryResultList<T1>();
        await queryResult.ReadResultsAsync(result.Value!);
        return queryResult;
    }
}
