

using FluentResults;
using Oasis.Application.DTOs.BookingDTO;

namespace Oasis.Application.Interfaces;


/// <summary>
/// 预订服务接口
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Creates multiple bookings asynchronously.
    /// </summary>
    /// <param name="bookings">The list of bookings to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> UploadBookingsAsync(List<CreateBookingRequest> bookings, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes all bookings asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> DeleteAllBookingsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Checks in a booking asynchronously.
    /// </summary>
    /// <param name="bookingId">The ID of the booking to check in.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> CheckInBookingAsync(long bookingId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks out a booking asynchronously.
    /// </summary>
    /// <param name="bookingId">The ID of the booking to check out.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> CheckOutBookingAsync(long bookingId, CancellationToken cancellationToken);


    /// <summary>
    /// Unconfirms a booking asynchronously.
    /// </summary>
    /// <param name="bookingId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> UnConfirmBookingAsync(long bookingId, CancellationToken cancellationToken);

}