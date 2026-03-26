using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oasis.Application.DTOs.CabinFeature;
using Oasis.Application.Interfaces;
using Oasis.Domain;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Infrastructure.Services;

/// <summary>
/// Cabin service implementation
/// </summary>
/// <param name="dbContext"></param>
/// <param name="logger"></param>
public sealed class CabinService(OasisDbContext dbContext, ILogger<CabinService> logger, IBatchOperationHandler<CreateCabinRequest, Cabin> batchAction) : ICabinService
{

    public async Task<Result> CreateCabinAsync(CreateCabinRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cabin = request.Adapt<Cabin>();
            dbContext.Cabins.Add(cabin);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.UnhandledException(ex);
            return Result.Fail("Failed to create cabin").WithError(ex.Message);
        }
    }


    public async ValueTask<Result> UpdateCabinAsync(long id, UpdateCabinRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var cabin = dbContext.Cabins.AsTracking().FirstOrDefault(c => c.Id == id);
            if (cabin is null) return Result.Fail("Cabin not found");
            //mapster update to existing entity
            var re = request.Adapt(cabin);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.UnhandledException(ex);
            return Result.Fail("Failed to update cabin").WithError(ex.Message);
        }
    }


    /// <summary>
    /// delete a cabin asynchronously
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result> DeleteCabinAsync(long id, CancellationToken cancellationToken)
    {
        var cabin = await dbContext.Cabins.AsTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (cabin == null)
        {
            return Result.Fail("Cabin not found");
        }

        dbContext.Cabins.Remove(cabin);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }


    /// <summary>
    /// create multiple cabins asynchronously
    /// </summary>
    /// <param name="cabins"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> UploadCabinsAsync(List<CreateCabinRequest> cabins, CancellationToken cancellationToken) => batchAction.BatchInsertAsync(dbContext, cabins, cancellationToken);


    /// <summary>
    /// delete all cabins asynchronously
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> DeleteAllCabinsAsync(CancellationToken cancellationToken) => batchAction.BatchDeleteAsync(dbContext, cancellationToken);


}