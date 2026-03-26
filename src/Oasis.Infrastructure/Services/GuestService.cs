using Oasis.Application.Interfaces;
using FluentResults;
using Oasis.Application.DTOs.GuestFeature;
using Oasis.Domain;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Infrastructure.Services;


public class GuestService(OasisDbContext dbContext, IBatchOperationHandler<CreateGuestRequest, Guest> batchAction) : IGuestService
{

    public Task<Result> UploadGuestsAsync(List<CreateGuestRequest> guests, CancellationToken cancellationToken) => batchAction.BatchInsertAsync(dbContext, guests, cancellationToken);

    public Task<Result> DeleteAllGuestsAsync(CancellationToken cancellationToken) => batchAction.BatchDeleteAsync(dbContext, cancellationToken);
}