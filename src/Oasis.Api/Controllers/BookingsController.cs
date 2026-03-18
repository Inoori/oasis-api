using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BookingsController(OasisDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    [Route("odata/bookings")]
    public async Task<IActionResult> Get()
    {
        return Ok(dbContext.Bookings.AsQueryable());
    }

    [HttpPost]
    [Route("~/odata/bookings/batch")]
    public async Task<IActionResult> CreateBookings(
        [FromBody] List<Booking> bookings,
        CancellationToken cancellationToken)
    {
        dbContext.Bookings.AddRange(bookings);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBookings(
            CancellationToken cancellationToken)
    {
        await dbContext.Bookings.ExecuteDeleteAsync(cancellationToken);
        return NoContent();
    }




    [HttpPost]
    [Route("{id}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] JsonElement element, CancellationToken cancellationToken)
    {

        if (!element.TryGetProperty("status", out var statusElement))
        {
            return BadRequest("Missing status property");
        }

        if (!Enum.TryParse<BookingStatus>(statusElement.GetString(), true, out var status))
        {
            return BadRequest("Invalid status value");
        }

        var existingBooking = await dbContext.Bookings.AsTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (existingBooking == null)
        {
            return NotFound();
        }

        existingBooking.Status = status;
        if (status == BookingStatus.CheckedIn)
        {
            existingBooking.IsPaid = true;
        }
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok();
    }
}
