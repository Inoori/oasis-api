using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oasis.Application.DTOs.BookingFeature;
using Oasis.Application.Interfaces;
using Oasis.Domain;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Infrastructure.Services;

/// <summary>
/// Booking service implementation
/// </summary>
/// <param name="dbContext"></param>
/// <param name="logger"></param>
/// <param name="batchAction"></param>
public sealed class BookingService(OasisDbContext dbContext, ILogger<BookingService> logger, IBatchOperationHandler<CreateBookingRequest, Booking> batchAction) : IBookingService
{
    /// <summary>
    /// checkin booking
    /// </summary>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public async Task<Result> CheckInBookingAsync(long bookingId, CancellationToken cancellationToken)
    {
        var booking = await dbContext.Bookings.AsTracking().FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null) return Result.Fail("Booking not found");

        var result = booking.CheckIn();
        if (result.IsFailed) return Result.Fail(result.Errors);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }


    /// <summary>
    /// checkout booking
    /// </summary>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result> CheckOutBookingAsync(long bookingId, CancellationToken cancellationToken)
    {
        var booking = await dbContext.Bookings.AsTracking().FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null) return Result.Fail("Booking not found");

        var result = booking.CheckOut();
        if (result.IsFailed) return Result.Fail(result.Errors);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }


    /// <summary>
    /// unconfirm a booking asynchronously.
    /// </summary>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result> UnConfirmBookingAsync(long bookingId, CancellationToken cancellationToken)
    {
        var booking = await dbContext.Bookings.AsTracking().FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null) return Result.Fail("Booking not found");

        booking.UnConfirm();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }


    /// <summary>
    /// Creates multiple bookings asynchronously.
    /// </summary>
    /// <param name="bookings"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> UploadBookingsAsync(List<CreateBookingRequest> bookings, CancellationToken cancellationToken) => batchAction.BatchInsertAsync(dbContext, bookings, cancellationToken);

    /// <summary>
    /// Deletes all bookings asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Result> DeleteAllBookingsAsync(CancellationToken cancellationToken) => batchAction.BatchDeleteAsync(dbContext, cancellationToken);
}