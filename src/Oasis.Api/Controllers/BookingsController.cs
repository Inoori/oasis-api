using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Oasis.Application.DTOs.BookingFeature;
using Oasis.Application.Interfaces;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingService bookingService) : ControllerBase
{

    [HttpPost("upload")]
    public async Task<IActionResult> UploadBookings(
        [FromBody] List<CreateBookingRequest> bookings,
        CancellationToken cancellationToken)
    {
        var result = await bookingService.UploadBookingsAsync(bookings, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToActionResult();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBookings(
            CancellationToken cancellationToken)
    {
        var result = await bookingService.DeleteAllBookingsAsync(cancellationToken);
        return result.IsSuccess ? Ok() : result.ToActionResult();
    }


    [HttpPost("{id}/checkin")]
    public async Task<IActionResult> CheckInBooking(long id, CancellationToken cancellationToken)
    {
        var result = await bookingService.CheckInBookingAsync(id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToActionResult();
    }

    [HttpPost("{id}/checkout")]
    public async Task<IActionResult> CheckOutBooking(long id, CancellationToken cancellationToken)
    {
        var result = await bookingService.CheckOutBookingAsync(id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToActionResult();
    }


    [HttpPost("{id}/unconfirm")]
    public async Task<IActionResult> UnConfirmBooking(long id, CancellationToken cancellationToken)
    {
        var result = await bookingService.UnConfirmBookingAsync(id, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToActionResult();
    }
}
