using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Oasis.Infrastructure.Persistence;

namespace Oasis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController(OasisDbContext dbContext) : ControllerBase

{
    [HttpGet]
    [Route("odata/guests")]
    [EnableQuery]
    public async Task<IActionResult> Get()
    {
        return Ok(dbContext.Guests.AsQueryable());
    }

    [HttpPost]
    [Route("~/odata/guests/batch")]
    public async Task<IActionResult> CreateGuests(
        [FromBody] List<Guest> guests,
        CancellationToken cancellationToken)
    {
        dbContext.Guests.AddRange(guests);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteGuests(
            CancellationToken cancellationToken)
    {
        await dbContext.Guests.ExecuteDeleteAsync(cancellationToken);
        return NoContent();
    }

}