using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Oasis.Application.Interfaces;
using Oasis.Infrastructure.Persistence;


namespace Oasis.Infrastructure.Services;


/// <summary>
/// Batch action service implementation
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public class BatchOperationHandler<TRequest, TEntity> : IBatchOperationHandler<TRequest, TEntity> where TEntity : class where TRequest : IMapFrom<TEntity>
{
    /// <summary>
    /// create multiple entities asynchronously
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result> BatchInsertAsync(DbContext dbContext, List<TRequest> entities, CancellationToken cancellationToken)
    {
        try
        {
            var obj = entities.Adapt<List<TEntity>>();
            dbContext.Set<TEntity>().AddRange(obj);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }



    /// <summary>
    /// delete multiple entities asynchronously
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result> BatchDeleteAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        try
        {
            await dbContext.Set<TEntity>().ExecuteDeleteAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}