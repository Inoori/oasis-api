
using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Oasis.Application.Interfaces;

/// <summary>
/// 可批量操作对象接口
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public interface IBatchOperationHandler<TRequest, TEntity> where TRequest : IMapFrom<TEntity>
{
    Task<Result> BatchInsertAsync(DbContext dbContext, List<TRequest> entities, CancellationToken cancellationToken);
    Task<Result> BatchDeleteAsync(DbContext dbContext, CancellationToken cancellationToken);
}