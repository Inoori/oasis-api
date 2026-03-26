using Microsoft.AspNetCore.Mvc;
using Oasis.Application.DTOs.GuestFeature;
using Oasis.Application.Interfaces;

namespace Oasis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController(IGuestService guestService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadGuests(
        [FromBody] List<CreateGuestRequest> guests,
        CancellationToken cancellationToken)
    {
        var result = await guestService.UploadGuestsAsync(guests, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken)
    {
        var result = await guestService.DeleteAllGuestsAsync(cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
    }
}